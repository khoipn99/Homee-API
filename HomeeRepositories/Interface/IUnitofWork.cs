using HomeeRepositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeeRepositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Chef> ChefRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Food> FoodRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Voucher> VoucherRepository { get; }
        IGenericRepository<TopUpRequest> TopUpRequestRepository { get; }
        void Save();
    }
}
