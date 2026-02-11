using System;

namespace SERVICES.Interfaces
{
    public interface IErrorHandlerService
    {
        void HandleError(Exception ex, string context = null);
        string GetFriendlyMessage(Exception ex);
        void ShowError(Exception ex, string context = null);
    }
}
