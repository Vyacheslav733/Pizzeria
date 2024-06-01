using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PizzeriaFileImplement.Models
{
    [DataContract]
    public class Implementer : IImplementerModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string ImplementerFIO { get; private set; } = string.Empty;

        [DataMember]
        public string Password { get; private set; } = string.Empty;

        [DataMember]
        public int WorkExperience { get; private set; }

        [DataMember]
        public int Qualification { get; private set; }

        public static Implementer? Create(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            return new()
            {
                ImplementerFIO = element.Element("FIO")!.Value,
                Password = element.Element("Password")!.Value,
                Id = Convert.ToInt32(element.Attribute("Id")!.Value),
                Qualification = Convert.ToInt32(element.Element("Qualification")!.Value),
                WorkExperience = Convert.ToInt32(element.Element("WorkExperience")!.Value),
            };
        }

        public static Implementer? Create(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new()
            {
                Id = model.Id,
                Password = model.Password,
                Qualification = model.Qualification,
                ImplementerFIO = model.ImplementerFIO,
                WorkExperience = model.WorkExperience,
            };
        }



        public void Update(ImplementerBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            Password = model.Password;
            Qualification = model.Qualification;
            ImplementerFIO = model.ImplementerFIO;
            WorkExperience = model.WorkExperience;
        }

        public ImplementerViewModel GetViewModel => new()
        {
            Id = Id,
            Password = Password,
            Qualification = Qualification,
            ImplementerFIO = ImplementerFIO,
        };

        public XElement GetXElement => new("Client",
            new XAttribute("Id", Id),
            new XElement("Password", Password),
            new XElement("FIO", ImplementerFIO),
            new XElement("Qualification", Qualification),
            new XElement("WorkExperience", WorkExperience)
            );
    }
}
