using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGrooming.Models;

namespace PetGrooming.BLL
{
    public interface IServiceBLL
    {
        void Create(Service s);
        void Update(Service s);
        void Delete(int serviceId);
        List<Service> GetAll();
        Service? GetById(int serviceId);
        Service? GetByName(string serviceName);
    }
}
