/*

http://www.brad-smith.info/blog/archives/104


Copyright (c) 2014, Bradley Smith
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * The name "Bradley Smith" may not be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL BRADLEY SMITH BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

/// <summary>
///     Attaches to a System.Windows.Forms.Control and provides buffered
///     painting functionality.
///     <para>
///         Uses TState to represent the visual state of the control. Animations
///         are attached to transitions between states.
///     </para>
/// </summary>
/// <typeparam name="TState">Any type representing the visual state of the control.</typeparam>
public class BufferedPainter<TState>
{
    private bool _animationsNeedCleanup;
    private TState _currentState;
    private TState _newState;
    private TState _defaultState;
    private Size _oldSize;

    /// <summary>
    ///     Fired when the control must be painted in a particular state.
    /// </summary>
    public event EventHandler<BufferedPaintEventArgs<TState>> PaintVisualState;

    /// <summary>
    ///     Raises the PaintVisualState event.
    /// </summary>
    /// <param name="e">BufferedPaintEventArgs instance.</param>
    protected virtual void OnPaintVisualState(BufferedPaintEventArgs<TState> e)
    {
        PaintVisualState?.Invoke(this, e);
    }

    /// <summary>
    ///     Gets whether buffered painting is supported for the current OS/configuration.
    /// </summary>
    public bool BufferedPaintSupported { get; }

    /// <summary>
    ///     Gets the control this instance is attached to.
    /// </summary>
    public Control Control { get; }

    /// <summary>
    ///     Gets or sets the default animation duration (in milliseconds) for state transitions. The default is zero (not
    ///     animated).
    /// </summary>
    public int DefaultDuration { get; set; }

    /// <summary>
    ///     Gets or sets whether animation is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///     Gets or sets the default visual state. The default value is 'default(TState)'.
    /// </summary>
    public TState DefaultState
    {
        get { return _defaultState; }
        set
        {
            bool usingOldDefault = Equals(_currentState, _defaultState);
            _defaultState = value;
            if (usingOldDefault) _currentState = _newState = _defaultState;
        }
    }

    /// <summary>
    ///     Gets the collection of state transitions and their animation durations.
    ///     Only one item for each unique state transition is permitted.
    /// </summary>
    public ICollection<BufferedPaintTransition<TState>> Transitions { get; }

    /// <summary>
    ///     Gets the collection of state change triggers.
    ///     Only one item for each unique combination of type and visual state is permitted.
    /// </summary>
    public ICollection<VisualStateTrigger<TState>> Triggers { get; }

    /// <summary>
    ///     Gets or sets the current visual state.
    /// </summary>
    public TState State
    {
        get { return _currentState; }
        set
        {
            bool diff = !Equals(_currentState, value);
            _newState = value;
            if (diff)
            {
                if (_animationsNeedCleanup && Control.IsHandleCreated)
                    Interop.BufferedPaintStopAllAnimations(Control.Handle);
                Control.Invalidate();
            }
        }
    }

    /// <summary>
    ///     Initialises a new instance of the BufferedPainter class.
    /// </summary>
    /// <param name="control">
    ///     Control this instance is attached to.
    ///     <para>
    ///         For best results, use a control which does not paint its background.
    ///     </para>
    ///     <para>
    ///         Note: Buffered painting does not work if the OptimizedDoubleBuffer flag is set for the control.
    ///     </para>
    /// </param>
    public BufferedPainter(Control control)
    {
        Transitions = new HashSet<BufferedPaintTransition<TState>>();
        Triggers = new HashSet<VisualStateTrigger<TState>>();

        Enabled = true;
        _defaultState = _currentState = _newState = default(TState);

        Control = control;
        _oldSize = Control.Size;

        // buffered painting requires Windows Vista and above with themes supported and enabled (i.e. Basic/Aero theme, not Classic)
        BufferedPaintSupported = IsSupported();

        Control.Resize += Control_Resize;
        Control.Disposed += Control_Disposed;
        Control.Paint += Control_Paint;
        Control.HandleCreated += Control_HandleCreated;

        Control.MouseEnter += (o, e) => EvalTriggers();
        Control.MouseLeave += (o, e) => EvalTriggers();
        Control.MouseMove += (o, e) => EvalTriggers();
        Control.GotFocus += (o, e) => EvalTriggers();
        Control.LostFocus += (o, e) => EvalTriggers();
        Control.MouseDown += (o, e) => EvalTriggers();
        Control.MouseUp += (o, e) => EvalTriggers();
    }

    /// <summary>
    ///     Returns a value indicating whether buffered painting is supported under the current OS and configuration.
    /// </summary>
    /// <returns></returns>
    internal static bool IsSupported()
    {
        return (Environment.OSVersion.Version.Major >= 6) && VisualStyleRenderer.IsSupported &&
               Application.RenderWithVisualStyles;
    }

    /// <summary>
    ///     Short-hand method for adding a state transition.
    /// </summary>
    /// <param name="fromState">The previous visual state.</param>
    /// <param name="toState">The new visual state.</param>
    /// <param name="duration">Duration of the animation (in milliseconds).</param>
    public void AddTransition(TState fromState, TState toState, int duration)
    {
        Transitions.Add(new BufferedPaintTransition<TState>(fromState, toState, duration));
    }

    /// <summary>
    ///     Short-hand method for adding a state change trigger.
    /// </summary>
    /// <param name="type">Type of trigger.</param>
    /// <param name="state">Visual state applied when the trigger occurs.</param>
    /// <param name="bounds">Bounds within which the trigger applies.</param>
    /// <param name="anchor">How the bounds are anchored to the control.</param>
    public void AddTrigger(VisualStateTriggerTypes type, TState state, Rectangle bounds = default(Rectangle),
        AnchorStyles anchor = AnchorStyles.Top | AnchorStyles.Left)
    {
        Triggers.Add(new VisualStateTrigger<TState>(type, state, bounds, anchor));
    }

    /// <summary>
    ///     Evaluates all state change triggers.
    /// </summary>
    private void EvalTriggers()
    {
        if (!Triggers.Any()) return;

        TState newState = DefaultState;

        ApplyCondition(VisualStateTriggerTypes.Focused, ref newState);
        ApplyCondition(VisualStateTriggerTypes.Hot, ref newState);
        ApplyCondition(VisualStateTriggerTypes.Pushed, ref newState);

        State = newState;
    }

    /// <summary>
    ///     Helper method for EvalTriggers().
    /// </summary>
    /// <param name="type">Type of trigger to search for.</param>
    /// <param name="stateIfTrue">Reference to the visual state variable to update (if the trigger occurs).</param>
    private void ApplyCondition(VisualStateTriggerTypes type, ref TState stateIfTrue)
    {
        foreach (VisualStateTrigger<TState> trigger in Triggers.Where(x => x.Type == type))
        {
            if (trigger != null)
            {
                Rectangle bounds = (trigger.Bounds != Rectangle.Empty) ? trigger.Bounds : Control.ClientRectangle;

                bool inRect = bounds.Contains(Control.PointToClient(Cursor.Position));
                bool other = true;

                switch (type)
                {
                    case VisualStateTriggerTypes.Focused:
                        other = Control.Focused;
                        inRect = true;
                        break;
                    case VisualStateTriggerTypes.Pushed:
                        other = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
                        break;
                }
                if (other && inRect) stateIfTrue = trigger.State;
            }
        }
    }

/*
    /// <summary>
    ///     Deactivates buffered painting.
    /// </summary>
    private void CleanupAnimations()
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke(new MethodInvoker(CleanupAnimations));
        }
        else if (_animationsNeedCleanup)
        {
            if (Control.IsHandleCreated) Interop.BufferedPaintStopAllAnimations(Control.Handle);
            Interop.BufferedPaintUnInit();
            _animationsNeedCleanup = false;
        }
    }
*/

private void Control_HandleCreated(object sender, EventArgs e)
    {
        if (BufferedPaintSupported)
        {
            Interop.BufferedPaintInit();
            _animationsNeedCleanup = true;
        }
    }

    private void Control_Disposed(object sender, EventArgs e)
    {
        if (_animationsNeedCleanup)
        {
            Interop.BufferedPaintUnInit();
            _animationsNeedCleanup = false;
        }
    }

    private void Control_Resize(object sender, EventArgs e)
    {
        // resizing stops all playing animations
        if (_animationsNeedCleanup && Control.IsHandleCreated) Interop.BufferedPaintStopAllAnimations(Control.Handle);

        // update trigger bounds according to anchor styles
        foreach (VisualStateTrigger<TState> trigger in Triggers)
        {
            if (trigger.Bounds != Rectangle.Empty)
            {
                Rectangle newBounds = trigger.Bounds;

                if ((trigger.Anchor & AnchorStyles.Left) != AnchorStyles.Left)
                {
                    newBounds.X += (Control.Width - _oldSize.Width);
                }

                if ((trigger.Anchor & AnchorStyles.Top) != AnchorStyles.Top)
                {
                    newBounds.Y += (Control.Height - _oldSize.Height);
                }

                if ((trigger.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                {
                    newBounds.Width += (Control.Width - _oldSize.Width);
                }

                if ((trigger.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                {
                    newBounds.Height += (Control.Height - _oldSize.Height);
                }

                trigger.Bounds = newBounds;
            }
        }

        // save old size for next resize
        _oldSize = Control.Size;
    }

    private void Control_Paint(object sender, PaintEventArgs e)
    {
        if (BufferedPaintSupported && Enabled)
        {
            bool stateChanged = !Equals(_currentState, _newState);

            IntPtr hdc = e.Graphics.GetHdc();
            if (hdc != IntPtr.Zero)
            {
                // see if this paint was generated by a soft-fade animation
                if (!Interop.BufferedPaintRenderAnimation(Control.Handle, hdc))
                {
                    Interop.BP_ANIMATIONPARAMS animParams = new Interop.BP_ANIMATIONPARAMS();
                    animParams.cbSize = Marshal.SizeOf(animParams);
                    animParams.style = Interop.BP_ANIMATIONSTYLE.BPAS_LINEAR;

                    // get appropriate animation time depending on state transition (or 0 if unchanged)
                    animParams.dwDuration = 0;
                    if (stateChanged)
                    {
                        BufferedPaintTransition<TState> transition =
                            Transitions.SingleOrDefault(
                                x => Equals(x.FromState, _currentState) && Equals(x.ToState, _newState));
                        animParams.dwDuration = transition?.Duration ?? DefaultDuration;
                    }

                    Rectangle rc = Control.ClientRectangle;
                    IntPtr hdcFrom, hdcTo;
                    IntPtr hbpAnimation = Interop.BeginBufferedAnimation(Control.Handle, hdc, ref rc,
                        Interop.BP_BUFFERFORMAT.BPBF_COMPATIBLEBITMAP, IntPtr.Zero, ref animParams, out hdcFrom,
                        out hdcTo);
                    if (hbpAnimation != IntPtr.Zero)
                    {
                        if (hdcFrom != IntPtr.Zero)
                        {
                            using (Graphics g = Graphics.FromHdc(hdcFrom))
                            {
                                OnPaintVisualState(new BufferedPaintEventArgs<TState>(_currentState, g));
                            }
                        }
                        if (hdcTo != IntPtr.Zero)
                        {
                            using (Graphics g = Graphics.FromHdc(hdcTo))
                            {
                                OnPaintVisualState(new BufferedPaintEventArgs<TState>(_newState, g));
                            }
                        }

                        _currentState = _newState;
                        Interop.EndBufferedAnimation(hbpAnimation, true);
                    }
                    else
                    {
                        OnPaintVisualState(new BufferedPaintEventArgs<TState>(_currentState, e.Graphics));
                    }
                }

                e.Graphics.ReleaseHdc(hdc);
            }
        }
        else
        {
            // buffered painting not supported, just paint using the current state
            _currentState = _newState;
            OnPaintVisualState(new BufferedPaintEventArgs<TState>(_currentState, e.Graphics));
        }
    }
}

/// <summary>
///     Represents a transition between two visual states. Describes the duration of the animation.
///     Two transitions are considered equal if they represent the same change in visual state.
/// </summary>
/// <typeparam name="TState">Any type representing the visual state of the control.</typeparam>
public class BufferedPaintTransition<TState> : IEquatable<BufferedPaintTransition<TState>>
{
    /// <summary>
    ///     Gets the previous visual state.
    /// </summary>
    public TState FromState { get; }

    /// <summary>
    ///     Gets the new visual state.
    /// </summary>
    public TState ToState { get; }

    /// <summary>
    ///     Gets or sets the duration (in milliseconds) of the animation.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    ///     Initialises a new instance of the BufferedPaintTransition class.
    /// </summary>
    /// <param name="fromState">The previous visual state.</param>
    /// <param name="toState">The new visual state.</param>
    /// <param name="duration">Duration of the animation (in milliseconds).</param>
    public BufferedPaintTransition(TState fromState, TState toState, int duration)
    {
        FromState = fromState;
        ToState = toState;
        Duration = duration;
    }

    /// <summary>
    ///     Determines if two instances are equal.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is BufferedPaintTransition<TState>)
            return ((IEquatable<BufferedPaintTransition<TState>>) this).Equals((BufferedPaintTransition<TState>) obj);
        else
            return base.Equals(obj);
    }

    /// <summary>
    ///     Serves as a hash function for a particular type.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return ((object) FromState ?? 0).GetHashCode() ^ ((object) ToState ?? 0).GetHashCode();
    }

    #region IEquatable<BufferedPaintAnimation<TState>> Members

    bool IEquatable<BufferedPaintTransition<TState>>.Equals(BufferedPaintTransition<TState> other)
    {
        return Equals(FromState, other.FromState) && Equals(ToState, other.ToState);
    }

    #endregion
}

/// <summary>
///     Represents a trigger for a particular visual state.
///     Two triggers are considered equal if they are of the same type and visual state.
/// </summary>
/// <typeparam name="TState">Any type representing the visual state of the control.</typeparam>
public class VisualStateTrigger<TState> : IEquatable<VisualStateTrigger<TState>>
{
    /// <summary>
    ///     Gets the type of trigger.
    /// </summary>
    public VisualStateTriggerTypes Type { get; }

    /// <summary>
    ///     Gets the visual state applied when the trigger occurs.
    /// </summary>
    public TState State { get; }

    /// <summary>
    ///     Gets or sets the bounds within which the trigger applies.
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    ///     Gets or sets how the bounds are anchored to the edge of the control.
    /// </summary>
    public AnchorStyles Anchor { get; set; }

    /// <summary>
    ///     Initialises a new instance of the VisualStateTrigger class.
    /// </summary>
    /// <param name="type">Type of trigger.</param>
    /// <param name="state">Visual state applied when the trigger occurs.</param>
    /// <param name="bounds">Bounds within which the trigger applies.</param>
    /// <param name="anchor">Anchor</param>
    public VisualStateTrigger(VisualStateTriggerTypes type, TState state, Rectangle bounds = default(Rectangle),
        AnchorStyles anchor = AnchorStyles.Top | AnchorStyles.Left)
    {
        Type = type;
        State = state;
        Bounds = bounds;
        Anchor = anchor;
    }

    /// <summary>
    ///     Determines if two instances are equal.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is BufferedPaintTransition<TState>)
            return ((IEquatable<VisualStateTrigger<TState>>) this).Equals((VisualStateTrigger<TState>) obj);
        else
            return base.Equals(obj);
    }

    /// <summary>
    ///     Serves as a hash function for a particular type.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Type.GetHashCode() ^ ((object) State ?? 0).GetHashCode();
    }

    #region IEquatable<VisualStateTrigger<TState>> Members

    bool IEquatable<VisualStateTrigger<TState>>.Equals(VisualStateTrigger<TState> other)
    {
        return (Type == other.Type) && Equals(State, other.State);
    }

    #endregion
}

/// <summary>
///     Represents the types of trigger which can change the visual state of a control.
/// </summary>
public enum VisualStateTriggerTypes
{
    /// <summary>
    ///     The control receives input focus.
    /// </summary>
    Focused,

    /// <summary>
    ///     The mouse is over the control.
    /// </summary>
    Hot,

    /// <summary>
    ///     The left mouse button is pressed on the control.
    /// </summary>
    Pushed
}

/// <summary>
///     EventArgs class for the BufferedPainter.PaintVisualState event.
/// </summary>
/// <typeparam name="TState">Any type representing the visual state of the control.</typeparam>
public class BufferedPaintEventArgs<TState> : EventArgs
{
    /// <summary>
    ///     Gets the visual state to paint.
    /// </summary>
    public TState State { get; }

    /// <summary>
    ///     Gets the Graphics object on which to paint.
    /// </summary>
    public Graphics Graphics { get; }

    /// <summary>
    ///     Initialises a new instance of the BufferedPaintEventArgs class.
    /// </summary>
    /// <param name="state">Visual state to paint.</param>
    /// <param name="graphics">Graphics object on which to paint.</param>
    public BufferedPaintEventArgs(TState state, Graphics graphics)
    {
        State = state;
        Graphics = graphics;
    }
}

/// <summary>
///     Represents a Windows combo box control that, when bound to a data source, is capable of
///     displaying items in groups/categories.
/// </summary>
[DesignerCategory("")]
public class GroupedComboBox : ComboBox, IComparer
{
    private BindingSource _bindingSource; // used for change detection and grouping
    private Font _groupFont; // for painting
    private string _groupMember; // name of group-by property
    private PropertyDescriptor _groupProperty; // used to get group-by values
    private ArrayList _internalItems; // internal sorted collection of items
    private BindingSource _internalSource; // binds sorted collection to the combobox
    private TextFormatFlags _textFormatFlags; // used in measuring/painting
    private BufferedPainter<ComboBoxState> _bufferedPainter; // provides buffered paint animations
    private bool _isNotDroppedDown;

    /// <summary>
    ///     Gets or sets the data source for this GroupedComboBox.
    /// </summary>
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.Repaint)]
    [AttributeProvider(typeof (IListSource))]
    public new object DataSource
    {
        get
        {
            // binding source should be transparent to the user
            return _bindingSource?.DataSource;
        }
        set
        {
            _internalSource = null;

            if (value != null)
            {
                // wrap the object in a binding source and listen for changes
                _bindingSource = new BindingSource(value, string.Empty);
                _bindingSource.ListChanged += mBindingSource_ListChanged;
                SyncInternalItems();
            }
            else
            {
                // remove binding
                base.DataSource = _bindingSource = null;
            }
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the drawing of elements in the list will be handled by user code.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DrawMode DrawMode => base.DrawMode;

    /// <summary>
    ///     Gets or sets the property to use when grouping items in the list.
    /// </summary>
    [DefaultValue("")]
    public string GroupMember
    {
        get { return _groupMember; }
        set
        {
            _groupMember = value;
            if (_bindingSource != null) SyncInternalItems();
        }
    }

    /// <summary>
    ///     Initialises a new instance of the GroupedComboBox class.
    /// </summary>
    public GroupedComboBox()
    {
        base.DrawMode = DrawMode.OwnerDrawVariable;
        _groupMember = string.Empty;
        _internalItems = new ArrayList();
        _textFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine |
                           TextFormatFlags.VerticalCenter;

        _bufferedPainter = new BufferedPainter<ComboBoxState>(this) {DefaultState = ComboBoxState.Normal};
        _bufferedPainter.PaintVisualState += _bufferedPainter_PaintVisualState;
        _bufferedPainter.AddTransition(ComboBoxState.Normal, ComboBoxState.Hot, 250);
        _bufferedPainter.AddTransition(ComboBoxState.Hot, ComboBoxState.Normal, 350);
        _bufferedPainter.AddTransition(ComboBoxState.Pressed, ComboBoxState.Normal, 350);

        ToggleStyle();
    }

    /// <summary>
    ///     Releases the resources used by the control.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        _bindingSource?.Dispose();
        _internalSource?.Dispose();

        base.Dispose(disposing);
    }

    /// <summary>
    ///     Recreates the control's handle when the DropDownStyle property changes.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDropDownStyleChanged(EventArgs e)
    {
        base.OnDropDownStyleChanged(e);
        ToggleStyle();
    }

    /// <summary>
    ///     Redraws the control when the dropdown portion is displayed.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDropDown(EventArgs e)
    {
        base.OnDropDown(e);
        _isNotDroppedDown = false;
        if (_bufferedPainter.Enabled) Invalidate();
    }

    /// <summary>
    ///     Redraws the control when the dropdown portion closes.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDropDownClosed(EventArgs e)
    {
        base.OnDropDownClosed(e);
        _isNotDroppedDown = true;
        if (_bufferedPainter.Enabled) Invalidate();
    }

    /// <summary>
    ///     Repaints the control when it receives input focus.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        if (_bufferedPainter.Enabled) Invalidate();
    }

    /// <summary>
    ///     Repaints the control when it loses input focus.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        if (_bufferedPainter.Enabled) Invalidate();
    }

    /// <summary>
    ///     Paints the control without a background (when using buffered painting).
    /// </summary>
    /// <param name="pevent"></param>
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        _bufferedPainter.State = GetRenderState();
    }

    /// <summary>
    ///     Redraws the control when the selected item changes.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnSelectedItemChanged(EventArgs e)
    {
        base.OnSelectedItemChanged(e);
        if (_bufferedPainter.Enabled) Invalidate();
    }

    /// <summary>
    ///     Explicit interface implementation for the IComparer.Compare method. Performs a two-tier comparison
    ///     on two list items so that the list can be sorted by group, then by display value.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    int IComparer.Compare(object x, object y)
    {
        // compare the display values (and return the result if there is no grouping)
        int secondLevelSort = Comparer.Default.Compare(GetItemText(x), GetItemText(y));
        if (_groupProperty == null) return secondLevelSort;

        // compare the group values - if equal, return the earlier comparison
        int firstLevelSort = Comparer.Default.Compare(
            Convert.ToString(_groupProperty.GetValue(x)),
            Convert.ToString(_groupProperty.GetValue(y))
            );

        return firstLevelSort == 0 ? secondLevelSort : firstLevelSort;
    }

    /// <summary>
    ///     Converts a ComboBoxState into its equivalent PushButtonState value.
    /// </summary>
    /// <param name="combo"></param>
    /// <returns></returns>
    private static PushButtonState GetPushButtonState(ComboBoxState combo)
    {
        switch (combo)
        {
            case ComboBoxState.Disabled:
                return PushButtonState.Disabled;
            case ComboBoxState.Hot:
                return PushButtonState.Hot;
            case ComboBoxState.Pressed:
                return PushButtonState.Pressed;
            default:
                return PushButtonState.Normal;
        }
    }

    /// <summary>
    ///     Determines the state in which to render the control (when using buffered painting).
    /// </summary>
    /// <returns></returns>
    private ComboBoxState GetRenderState()
    {
        if (!Enabled)
        {
            return ComboBoxState.Disabled;
        }
        else if (DroppedDown && !_isNotDroppedDown)
        {
            return ComboBoxState.Pressed;
        }
        else if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            if (((MouseButtons & MouseButtons.Left) == MouseButtons.Left) && !_isNotDroppedDown)
            {
                return ComboBoxState.Pressed;
            }
            else
            {
                return ComboBoxState.Hot;
            }
        }
        else
        {
            return ComboBoxState.Normal;
        }
    }

    /// <summary>
    ///     Determines whether the list item at the specified index is the start of a new group. In all
    ///     cases, populates the string respresentation of the group that the item belongs to.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="groupText"></param>
    /// <returns></returns>
    private bool IsGroupStart(int index, out string groupText)
    {
        bool isGroupStart = false;
        groupText = string.Empty;

        if ((_groupProperty != null) && (index >= 0) && (index < Items.Count))
        {
            // get the group value using the property descriptor
            groupText = Convert.ToString(_groupProperty.GetValue(Items[index]));

            // this item is the start of a group if it is the first item with a group -or- if
            // the previous item has a different group
            if ((index == 0) && (groupText != string.Empty))
            {
                isGroupStart = true;
            }
            else if ((index - 1) >= 0)
            {
                string previousGroupText = Convert.ToString(_groupProperty.GetValue(Items[index - 1]));
                if (previousGroupText != groupText) isGroupStart = true;
            }
        }

        return isGroupStart;
    }

    /// <summary>
    ///     Re-synchronises the internal sorted collection when the data source changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mBindingSource_ListChanged(object sender, ListChangedEventArgs e)
    {
        SyncInternalItems();
    }

    /// <summary>
    ///     When the control font changes, updates the font used to render group names.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        _groupFont = new Font(Font, FontStyle.Bold);
    }

    /// <summary>
    ///     When the parent control changes, updates the font used to render group names.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        _groupFont = new Font(Font, FontStyle.Bold);
    }

    /// <summary>
    ///     Performs custom painting for a list item.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        base.OnDrawItem(e);

        if ((e.Index >= 0) && (e.Index < Items.Count))
        {
            // get noteworthy states
            bool comboBoxEdit = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool noAccelerator = (e.State & DrawItemState.NoAccelerator) == DrawItemState.NoAccelerator;
            bool disabled = (e.State & DrawItemState.Disabled) == DrawItemState.Disabled;
            bool focus = (e.State & DrawItemState.Focus) == DrawItemState.Focus;

            // determine grouping
            string groupText;
            bool isGroupStart = IsGroupStart(e.Index, out groupText) && !comboBoxEdit;
            bool hasGroup = (groupText != string.Empty) && !comboBoxEdit;

            // the item text will appear in a different colour, depending on its state
            Color textColor;
            if (disabled)
                textColor = SystemColors.GrayText;
            else if (!comboBoxEdit && selected)
                textColor = SystemColors.HighlightText;
            else
                textColor = ForeColor;

            // items will be indented if they belong to a group
            Rectangle itemBounds = Rectangle.FromLTRB(
                e.Bounds.X + (hasGroup ? 12 : 0),
                e.Bounds.Y + (isGroupStart ? (e.Bounds.Height / 2) : 0),
                e.Bounds.Right,
                e.Bounds.Bottom
                );
            Rectangle groupBounds = new Rectangle(
                e.Bounds.X,
                e.Bounds.Y,
                e.Bounds.Width,
                e.Bounds.Height / 2
                );

            if (isGroupStart && selected)
            {
                // ensure that the group header is never highlighted
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), groupBounds);
            }
            else if (disabled)
            {
                // disabled appearance
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            }
            else if (!comboBoxEdit)
            {
                // use the default background-painting logic
                e.DrawBackground();
            }

            // render group header text
            if (isGroupStart)
                TextRenderer.DrawText(
                    e.Graphics,
                    groupText,
                    _groupFont,
                    groupBounds,
                    ForeColor,
                    _textFormatFlags
                    );

            // render item text
            TextRenderer.DrawText(
                e.Graphics,
                GetItemText(Items[e.Index]),
                Font,
                itemBounds,
                textColor,
                _textFormatFlags
                );

            // paint the focus rectangle if required
            if (focus && !noAccelerator)
            {
                if (isGroupStart && selected)
                {
                    // don't draw the focus rectangle around the group header
                    ControlPaint.DrawFocusRectangle(e.Graphics,
                        Rectangle.FromLTRB(groupBounds.X, itemBounds.Y, itemBounds.Right, itemBounds.Bottom));
                }
                else
                {
                    // use default focus rectangle painting logic
                    e.DrawFocusRectangle();
                }
            }
        }
    }

    /// <summary>
    ///     Determines the size of a list item.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMeasureItem(MeasureItemEventArgs e)
    {
        base.OnMeasureItem(e);

        e.ItemHeight = Font.Height;

        string groupText;
        if (IsGroupStart(e.Index, out groupText))
        {
            // the first item in each group will be twice as tall in order to accommodate the group header
            e.ItemHeight *= 2;
            e.ItemWidth = Math.Max(
                e.ItemWidth,
                TextRenderer.MeasureText(
                    e.Graphics,
                    groupText,
                    _groupFont,
                    new Size(e.ItemWidth, e.ItemHeight),
                    _textFormatFlags
                    ).Width
                );
        }
    }

    /// <summary>
    ///     Rebuilds the internal sorted collection.
    /// </summary>
    private void SyncInternalItems()
    {
        // locate the property descriptor that corresponds to the value of GroupMember
        _groupProperty = null;
        foreach (
            PropertyDescriptor descriptor in
                _bindingSource.GetItemProperties(null)
                    .Cast<PropertyDescriptor>()
                    .Where(descriptor => descriptor.Name.Equals(_groupMember)))
        {
            _groupProperty = descriptor;
            break;
        }

        // rebuild the collection and sort using custom logic
        _internalItems.Clear();
        foreach (object item in _bindingSource) _internalItems.Add(item);
        _internalItems.Sort(this);

        // bind the underlying ComboBox to the sorted collection
        if (_internalSource == null)
        {
            _internalSource = new BindingSource(_internalItems, string.Empty);
            base.DataSource = _internalSource;
        }
        else
        {
            _internalSource.ResetBindings(false);
        }
    }

    /// <summary>
    ///     Changes the control style to allow user-painting in DropDownList mode (when using buffered painting).
    /// </summary>
    protected void ToggleStyle()
    {
        if (_bufferedPainter != null && _bufferedPainter.BufferedPaintSupported && (DropDownStyle == ComboBoxStyle.DropDownList))
        {
            _bufferedPainter.Enabled = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        else
        {
            if(_bufferedPainter != null) _bufferedPainter.Enabled = false;
            SetStyle(ControlStyles.UserPaint, false);
            SetStyle(ControlStyles.AllPaintingInWmPaint, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
        }

        if (IsHandleCreated) RecreateHandle();
    }

    /// <summary>
    ///     Draws a combo box in the Windows Vista (and newer) style.
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="bounds"></param>
    /// <param name="state"></param>
    internal static void DrawComboBox(Graphics graphics, Rectangle bounds, ComboBoxState state)
    {
        Rectangle comboBounds = bounds;
        comboBounds.Inflate(1, 1);
        ButtonRenderer.DrawButton(graphics, comboBounds, GetPushButtonState(state));

        Rectangle buttonBounds = new Rectangle(
            bounds.Left + (bounds.Width - 17),
            bounds.Top,
            17,
            bounds.Height - (state != ComboBoxState.Pressed ? 1 : 0)
            );

        Rectangle buttonClip = buttonBounds;
        buttonClip.Inflate(-2, -2);

        using (Region oldClip = graphics.Clip.Clone())
        {
            graphics.SetClip(buttonClip, CombineMode.Intersect);
            ComboBoxRenderer.DrawDropDownButton(graphics, buttonBounds, state);
            graphics.SetClip(oldClip, CombineMode.Replace);
        }
    }

    /// <summary>
    ///     Paints the control (using the Buffered Paint API).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void _bufferedPainter_PaintVisualState(object sender, BufferedPaintEventArgs<ComboBoxState> e)
    {
        VisualStyleRenderer r = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
        r.DrawParentBackground(e.Graphics, ClientRectangle, this);

        DrawComboBox(e.Graphics, ClientRectangle, e.State);

        Rectangle itemBounds = new Rectangle(0, 0, Width - 21, Height);
        itemBounds.Inflate(-1, -3);
        itemBounds.Offset(2, 0);

        // draw the item in the editable portion
        DrawItemState state = DrawItemState.ComboBoxEdit;
        if (Focused && ShowFocusCues && !DroppedDown) state |= DrawItemState.Focus;
        if (!Enabled) state |= DrawItemState.Disabled;
        OnDrawItem(new DrawItemEventArgs(e.Graphics, Font, itemBounds, SelectedIndex, state));
    }
}

internal static class Interop
{
    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintInit();

    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintUnInit();

    [DllImport("uxtheme")]
    public static extern IntPtr BeginBufferedAnimation(
        IntPtr hwnd,
        IntPtr hdcTarget,
        ref Rectangle rcTarget,
        BP_BUFFERFORMAT dwFormat,
        IntPtr pPaintParams,
        ref BP_ANIMATIONPARAMS pAnimationParams,
        out IntPtr phdcFrom,
        out IntPtr phdcTo
        );

    [DllImport("uxtheme")]
    public static extern IntPtr EndBufferedAnimation(IntPtr hbpAnimation, bool fUpdateTarget);

    [DllImport("uxtheme")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BufferedPaintRenderAnimation(IntPtr hwnd, IntPtr hdcTarget);

    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintStopAllAnimations(IntPtr hwnd);

    [StructLayout(LayoutKind.Sequential)]
    internal struct BP_ANIMATIONPARAMS
    {
        public int cbSize, dwFlags;
        public BP_ANIMATIONSTYLE style;
        public int dwDuration;
    }

    internal enum BP_BUFFERFORMAT
    {
        BPBF_COMPATIBLEBITMAP,
        BPBF_DIB,
        BPBF_TOPDOWNDIB,
        BPBF_TOPDOWNMONODIB
    }

    [Flags]
    internal enum BP_ANIMATIONSTYLE
    {
        BPAS_NONE = 0,
        BPAS_LINEAR = 1,
        BPAS_CUBIC = 2,
        BPAS_SINE = 3
    }
}