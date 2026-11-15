using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.Frontend.DungChung.Statess
{
    public class StateXem : IFrmState
    {
        public void Handle(IStateableForm form)
        {
            form.AllowInput(false);          // Đóng tất cả ô nhập
            form.AllowEditMaPhieu(false);    // Khóa mã phiếu
            // canAdd: true, canSave: false, canEdit: true, canDelete: true
            form.SetToolbarButtons(true, false, true, true);
        }
    }
}
