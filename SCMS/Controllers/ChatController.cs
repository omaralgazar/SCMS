using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.BL.BLInterfaces;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly AppDbContext _context;

        public ChatController(IChatService chatService, AppDbContext context)
        {
            _chatService = chatService;
            _context = context;
        }

        private int CurrentUserId()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            return int.TryParse(userIdStr, out var id) ? id : 0;
        }

        private UserType CurrentUserType()
        {
            var t = HttpContext.Session.GetInt32("UserType");
            return t.HasValue ? (UserType)t.Value : UserType.User;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = CurrentUserId();
            var type = CurrentUserType();
            if (userId == 0) return RedirectToAction("Login", "Account");

            if (type == UserType.Patient)
            {
                var threads = _chatService.GetThreadsForPatient(userId);
                return View(threads);
            }

            if (type == UserType.Receptionist)
            {
                var receptionistId = _context.Set<Receptionist>()
                    .Where(r => r.UserId == userId)
                    .Select(r => r.UserId)
                    .FirstOrDefault();

                if (receptionistId == 0) return NotFound();

                var threads = _chatService.GetThreadsForReceptionist(receptionistId);
                return View(threads);
            }

            return Forbid();
        }

        [HttpGet]
        public IActionResult Conversation(int id)
        {
            var userId = CurrentUserId();
            var type = CurrentUserType();
            if (userId == 0) return RedirectToAction("Login", "Account");

            var thread = _chatService.GetThreadById(id);
            if (thread == null) return NotFound();

            if (!CanAccessThread(type, userId, thread))
                return Forbid();

            var messages = _chatService.GetMessages(id);

            var receptionistName = thread.Receptionist != null
                ? thread.Receptionist.FullName
                : "Customer Care Supervisor";

            var vm = new ChatConversationVm
            {
                ConversationId = thread.ThreadId,
                AgentName = receptionistName,
                Messages = messages.Select(m => new ChatMessageVm
                {
                    MessageId = m.MessageId,
                    SenderName = m.SenderUser?.FullName ?? "Unknown",
                    IsFromCurrentUser = m.SenderUserId == userId,
                    Text = m.Content,
                    SentAt = m.SentAt
                }).ToList()
            };

            _chatService.MarkThreadAsRead(id, userId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(ChatConversationVm vm)
        {
            var userId = CurrentUserId();
            var type = CurrentUserType();
            if (userId == 0) return RedirectToAction("Login", "Account");

            if (vm.ConversationId <= 0) return BadRequest();

            var thread = _chatService.GetThreadById(vm.ConversationId);
            if (thread == null) return NotFound();

            if (!CanAccessThread(type, userId, thread))
                return Forbid();

            var receiverUserId = ResolveReceiverUserId(type, thread);

            _chatService.SendMessage(
                threadId: vm.ConversationId,
                senderUserId: userId,
                content: vm.NewMessageText,
                receiverUserId: receiverUserId
            );

            return RedirectToAction(nameof(Conversation), new { id = vm.ConversationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Start()
        {
            var userId = CurrentUserId();
            var type = CurrentUserType();
            if (userId == 0) return RedirectToAction("Login", "Account");

            if (type != UserType.Patient) return Forbid();

            var thread = _chatService.GetOrCreateThread(patientId: userId, receptionistId: null);
            return RedirectToAction(nameof(Conversation), new { id = thread.ThreadId });
        }

        private bool CanAccessThread(UserType type, int userId, ChatThread thread)
        {
            if (type == UserType.Patient)
                return thread.PatientId == userId;

            if (type == UserType.Receptionist)
                return thread.ReceptionistId.HasValue && thread.ReceptionistId.Value == userId;

            return false;
        }

        private int? ResolveReceiverUserId(UserType type, ChatThread thread)
        {
            if (type == UserType.Patient)
                return thread.ReceptionistId;

            if (type == UserType.Receptionist)
                return thread.PatientId;

            return null;
        }
    }
}
