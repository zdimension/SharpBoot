// A ComboBox Control With Grouping
// Bradley Smith - 2010/06/24 (updated 2015/04/14)

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BufferedPainting;
using SharpBoot;

/// <summary>
///     Represents a Windows combo box control that, when bound to a data source, is capable of
///     displaying items in groups/categories.
/// </summary>
[DesignerCategory("")]
public class GroupedComboBox : ComboBox, IComparer
{
    private BindingSource _bindingSource; // used for change detection and grouping
    private readonly BufferedPainter<ComboBoxState> _bufferedPainter; // provides buffered paint animations
    private Font _groupFont; // for painting
    private string _groupMember; // name of group-by property
    private PropertyDescriptor _groupProperty; // used to get group-by values
    private readonly ArrayList _internalItems; // internal sorted collection of items
    private BindingSource _internalSource; // binds sorted collection to the combobox
    private bool _isNotDroppedDown;
    private IComparer _sortComparer;
    private readonly TextFormatFlags _textFormatFlags; // used in measuring/painting

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
        _sortComparer = Comparer.Default;

        if (Program.IsWin)
        {
            _bufferedPainter = new BufferedPainter<ComboBoxState>(this);
            _bufferedPainter.DefaultState = ComboBoxState.Normal;
            _bufferedPainter.PaintVisualState +=
                _bufferedPainter_PaintVisualState;
            _bufferedPainter.AddTransition(ComboBoxState.Normal, ComboBoxState.Hot, 250);
            _bufferedPainter.AddTransition(ComboBoxState.Hot, ComboBoxState.Normal, 350);
            _bufferedPainter.AddTransition(ComboBoxState.Pressed, ComboBoxState.Normal, 350);
        }

        ToggleStyle();
    }

    /// <summary>
    ///     Gets or sets the data source for this GroupedComboBox.
    /// </summary>
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.Repaint)]
    [AttributeProvider(typeof(IListSource))]
    public new object DataSource
    {
        // binding source should be transparent to the user
        get => _bindingSource != null ? _bindingSource.DataSource : null;
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
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DrawMode DrawMode => base.DrawMode;

    /// <summary>
    ///     Gets or sets the property to use when grouping items in the list.
    /// </summary>
    [DefaultValue("")]
    public string GroupMember
    {
        get => _groupMember;
        set
        {
            _groupMember = value;
            if (_bindingSource != null) SyncInternalItems();
        }
    }

    /// <summary>
    ///     Gets or sets an implementation of the <see cref="IComparer" /> interface
    ///     that sorts the items in the control. It will be applied separately to
    ///     the group headings. The default value is <see cref="Comparer.Default" />.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IComparer SortComparer
    {
        get => _sortComparer;
        set
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value == this) throw new ArgumentException("The owning control cannot be used as a comparer.", "value");

            if (_sortComparer != value)
            {
                _sortComparer = value;
                if (_bindingSource != null) SyncInternalItems();
            }
        }
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
        var secondLevelSort = _sortComparer.Compare(GetItemText(x), GetItemText(y));
        if (_groupProperty == null) return secondLevelSort;

        // compare the group values - if equal, return the earlier comparison
        var firstLevelSort = _sortComparer.Compare(
            Convert.ToString(_groupProperty.GetValue(x)),
            Convert.ToString(_groupProperty.GetValue(y))
        );

        if (firstLevelSort == 0)
            return secondLevelSort;
        return firstLevelSort;
    }

    /// <summary>
    ///     Releases the resources used by the control.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (_bindingSource != null) _bindingSource.Dispose();
        if (_internalSource != null) _internalSource.Dispose();

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
        if (!Enabled) return ComboBoxState.Disabled;

        if (DroppedDown && !_isNotDroppedDown) return ComboBoxState.Pressed;

        if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            if ((MouseButtons & MouseButtons.Left) == MouseButtons.Left && !_isNotDroppedDown)
                return ComboBoxState.Pressed;
            return ComboBoxState.Hot;
        }

        return ComboBoxState.Normal;
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
        var isGroupStart = false;
        groupText = string.Empty;

        if (_groupProperty != null && index >= 0 && index < Items.Count)
        {
            // get the group value using the property descriptor
            groupText = Convert.ToString(_groupProperty.GetValue(Items[index]));

            // this item is the start of a group if it is the first item with a group -or- if
            // the previous item has a different group
            if (index == 0 && groupText != string.Empty)
            {
                isGroupStart = true;
            }
            else if (index - 1 >= 0)
            {
                var previousGroupText = Convert.ToString(_groupProperty.GetValue(Items[index - 1]));
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

        if (e.Index >= 0 && e.Index < Items.Count)
        {
            // get noteworthy states
            var comboBoxEdit = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var noAccelerator = (e.State & DrawItemState.NoAccelerator) == DrawItemState.NoAccelerator;
            var disabled = (e.State & DrawItemState.Disabled) == DrawItemState.Disabled;
            var focus = (e.State & DrawItemState.Focus) == DrawItemState.Focus;

            // determine grouping
            string groupText;
            var isGroupStart = IsGroupStart(e.Index, out groupText) && !comboBoxEdit;
            var hasGroup = groupText != string.Empty && !comboBoxEdit;

            // the item text will appear in a different colour, depending on its state
            Color textColor;
            if (disabled)
                textColor = SystemColors.GrayText;
            else if (!comboBoxEdit && selected)
                textColor = SystemColors.HighlightText;
            else
                textColor = ForeColor;

            // items will be indented if they belong to a group
            var itemBounds = Rectangle.FromLTRB(
                e.Bounds.X + (hasGroup ? 12 : 0),
                e.Bounds.Y + (isGroupStart ? e.Bounds.Height / 2 : 0),
                e.Bounds.Right,
                e.Bounds.Bottom
            );
            var groupBounds = new Rectangle(
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
                    // don't draw the focus rectangle around the group header
                    ControlPaint.DrawFocusRectangle(e.Graphics,
                        Rectangle.FromLTRB(groupBounds.X, itemBounds.Y, itemBounds.Right, itemBounds.Bottom));
                else
                    // use default focus rectangle painting logic
                    e.DrawFocusRectangle();
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
        foreach (PropertyDescriptor descriptor in _bindingSource.GetItemProperties(null))
            if (descriptor.Name.Equals(_groupMember))
            {
                _groupProperty = descriptor;
                break;
            }

        // rebuild the collection and sort using custom logic
        _internalItems.Clear();
        foreach (var item in _bindingSource) _internalItems.Add(item);
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
        if (Program.IsWin && _bufferedPainter != null && _bufferedPainter.BufferedPaintSupported &&
            DropDownStyle == ComboBoxStyle.DropDownList)
        {
            _bufferedPainter.Enabled = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        else
        {
            if (Program.IsWin && _bufferedPainter != null) _bufferedPainter.Enabled = false;
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
        var comboBounds = bounds;
        comboBounds.Inflate(1, 1);
        ButtonRenderer.DrawButton(graphics, comboBounds, GetPushButtonState(state));

        var buttonBounds = new Rectangle(
            bounds.Left + (bounds.Width - 17),
            bounds.Top,
            17,
            bounds.Height - (state != ComboBoxState.Pressed ? 1 : 0)
        );

        var buttonClip = buttonBounds;
        buttonClip.Inflate(-2, -2);

        using (var oldClip = graphics.Clip.Clone())
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
        var r = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
        r.DrawParentBackground(e.Graphics, ClientRectangle, this);

        DrawComboBox(e.Graphics, ClientRectangle, e.State);

        var itemBounds = new Rectangle(0, 0, Width - 21, Height);
        itemBounds.Inflate(-1, -3);
        itemBounds.Offset(2, 0);

        // draw the item in the editable portion
        var state = DrawItemState.ComboBoxEdit;
        if (Focused && ShowFocusCues && !DroppedDown) state |= DrawItemState.Focus;
        if (!Enabled) state |= DrawItemState.Disabled;
        OnDrawItem(new DrawItemEventArgs(e.Graphics, Font, itemBounds, SelectedIndex, state));
    }
}