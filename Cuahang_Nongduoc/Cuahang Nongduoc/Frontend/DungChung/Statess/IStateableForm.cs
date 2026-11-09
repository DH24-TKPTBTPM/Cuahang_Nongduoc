using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.Frontend.DungChung.Statess
{
   public interface IStateableForm
    {
        void AllowInput(bool enable);
        void AllowEditMaPhieu(bool enable);
        void SetToolbarButtons(bool canAdd, bool canSave, bool canEdit, bool canDelete);
    }
}
