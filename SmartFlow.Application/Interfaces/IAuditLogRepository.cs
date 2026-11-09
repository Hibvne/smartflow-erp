using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
    }
}
