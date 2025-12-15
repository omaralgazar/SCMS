using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IChatService
    {
        ChatThread GetOrCreateThread(int patientId, int? receptionistId = null);

        ChatThread? GetThreadById(int threadId);
        List<ChatThread> GetThreadsForPatient(int patientId);
        List<ChatThread> GetThreadsForReceptionist(int receptionistId);

        List<ChatMessage> GetMessages(int threadId, int take = 100, int skip = 0);
        ChatMessage? SendMessage(int threadId, int senderUserId, string content, int? receiverUserId = null);

        int GetUnreadCountForUser(int userId);
        bool MarkThreadAsRead(int threadId, int userId);

        bool CloseThread(int threadId);
        bool ReopenThread(int threadId);
    }
}
