using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDatabaseImplement.Implements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        public List<MessageInfoViewModel> GetFullList()
        {
            using var context = new PizzeriaDatabase();
            return context.MessageInfos.Select(x => x.GetViewModel).ToList();
        }
        public List<MessageInfoViewModel> GetFilteredList(MessageInfoSearchModel model)
        {
            if (!model.ClientId.HasValue && !model.PageLength.HasValue && !model.PageIndex.HasValue)
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            IEnumerable<MessageInfo> request = context.MessageInfos.Where(x => !x.IsReply);
            if (model.ClientId.HasValue)
                request = request.Where(x => x.ClientId.HasValue && x.ClientId == model.ClientId);
            if (model.PageLength.HasValue)
            {
                int skipRows = model.PageIndex.HasValue ? (model.PageIndex.Value - 1) * model.PageLength.Value : 0;
                request = request.Skip(skipRows).Take(model.PageLength.Value);
            }

            return request.Select(x => x.GetViewModel).ToList();
        }

        public MessageInfoViewModel? GetElement(MessageInfoSearchModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId))
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.MessageInfos.FirstOrDefault(x => x.MessageId == model.MessageId)?.GetViewModel;
        }
        public MessageInfoViewModel? Insert(MessageInfoBindingModel model)
        {
            var newMessage = MessageInfo.Create(model);
            if (newMessage == null)
            {
                return null;
            }
            using var context = new PizzeriaDatabase();
            try
            {
                context.MessageInfos.Add(newMessage);
                context.SaveChanges();
                return newMessage.GetViewModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MessageInfoViewModel? Update(MessageInfoBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var message = context.MessageInfos.FirstOrDefault(x => x.MessageId == model.MessageId);
            if (message == null)
            {
                return null;
            }
            message.Update(context, model);
            context.SaveChanges();
            return message.GetViewModel;
        }
    }
}
