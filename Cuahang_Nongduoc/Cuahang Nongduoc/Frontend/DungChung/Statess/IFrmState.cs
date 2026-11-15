using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.Frontend.DungChung.Statess
{
    public interface IFrmState
    {
        void Handle(IStateableForm form);
    }
}
