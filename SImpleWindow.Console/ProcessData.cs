
namespace SimpleWindow.Console
{
    public class ProcessData
    {
        private uint _id;
        public uint Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
            }
        }

        private string _mainWindowTitle;
        public string MainWindowTitle
        {
            get { return _mainWindowTitle; }
            set
            {
                if (value == _mainWindowTitle) return;
                _mainWindowTitle = value;
            }
        }
    }
}
