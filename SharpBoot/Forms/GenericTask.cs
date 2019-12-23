using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBoot.Forms
{
    public sealed class GenericTask : WorkerFrm
    {
        public GenericTask(string title)
        {
            Text = title;
        }

        public Action WorkHandler { get; set; }

        public override void DoWork()
        {
            WorkHandler?.Invoke();
        }
    }
}
