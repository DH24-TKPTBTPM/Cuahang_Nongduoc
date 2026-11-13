using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.Frontend.DungChung.Statess
{
    public class StateThem : IFrmState
    {
        public void Handle(IStateableForm form)
        {
            form.AllowInput(true);           // Mở các ô nhập dữ liệu
            form.AllowEditMaPhieu(true);     // Cho phép nhập Mã phiếu mới
            // canAdd: false, canSave: true, canEdit: false, canDelete: false
            form.SetToolbarButtons(false, true, false, false);
        }
    }
}
