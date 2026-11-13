// ==================== [D06] REFACTORING ====================
// Kỹ thuật         : Strategy Pattern (GoF)
// Chức năng        : Bộ Validator nghiệp vụ
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject.Validation
{
    public class ValidatorContext
    {
        private readonly List<IValidationStrategy> m_Strategies = new List<IValidationStrategy>();

        public void AddStrategy(IValidationStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException("strategy");

            m_Strategies.Add(strategy);
        }

        public bool ValidateAll(object data, out string errorMessage)
        {
            StringBuilder errors = new StringBuilder();
            bool isValid = true;

            foreach (IValidationStrategy strategy in m_Strategies)
            {
                string strategyError;
                if (!strategy.Validate(data, out strategyError))
                {
                    isValid = false;
                    if (errors.Length > 0)
                        errors.AppendLine();
                    errors.Append(strategyError);
                }
            }

            errorMessage = errors.ToString();
            return isValid;
        }
    }
}
