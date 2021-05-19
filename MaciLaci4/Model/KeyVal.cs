using System;
using System.Collections.Generic;
using System.Text;

namespace MaciLaci4.Model
{
    public class KeyVal<Key, Val>
    {
        private Key _id;
        private Val _pos;

        public Key Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public Val Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public KeyVal() { }

        public KeyVal(Key key, Val val)
        {
            Id = key;
            Position = val;
        }

    }
}
