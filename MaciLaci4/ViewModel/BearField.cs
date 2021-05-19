using System;
using System.Collections.Generic;
using System.Text;

namespace MaciLaci4.ViewModel
{
    class BearField : ViewModelBase
    {
        private int _type;

        public int Type { get => _type; set { _type = value; OnPropertyChanged(); } }

        internal BearViewModel BearViewModel
        {
            get => default;
            set
            {
            }
        }

        public BearField(int type)
        {
            Type = type;
        }
    }
}