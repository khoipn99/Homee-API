using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeeRepositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeeContext _context;

        private IGenericRepository<Category> categoryRepository;
        private IGenericRepository<Chef> chefRepository;
        private IGenericRepository<Comment> commentRepository;
        private IGenericRepository<Food> foodRepository;
        private IGenericRepository<Order> orderRepository;
        private IGenericRepository<OrderDetail> orderDetailRepository;
        private IGenericRepository<Payment> paymentRepository;
        private IGenericRepository<Role> roleRepository;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Voucher> voucherRepository;
        private IGenericRepository<TopUpRequest> topUpRequestRepository;

        public UnitOfWork(HomeeContext context)
        {
            _context = context;
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                return categoryRepository ??= new GenericRepository<Category>(_context);
            }
        }

        public IGenericRepository<Chef> ChefRepository
        {
            get
            {
                return chefRepository ??= new GenericRepository<Chef>(_context);
            }
        }

        public IGenericRepository<Comment> CommentRepository
        {
            get
            {
                return commentRepository ??= new GenericRepository<Comment>(_context);
            }
        }

        public IGenericRepository<Food> FoodRepository
        {
            get
            {
                return foodRepository ??= new GenericRepository<Food>(_context);
            }
        }

        public IGenericRepository<Order> OrderRepository
        {
            get
            {
                return orderRepository ??= new GenericRepository<Order>(_context);
            }
        }

        public IGenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                return orderDetailRepository ??= new GenericRepository<OrderDetail>(_context);
            }
        }

        public IGenericRepository<Payment> PaymentRepository
        {
            get
            {
                return paymentRepository ??= new GenericRepository<Payment>(_context);
            }
        }

        public IGenericRepository<Role> RoleRepository
        {
            get
            {
                return roleRepository ??= new GenericRepository<Role>(_context);
            }
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                return userRepository ??= new GenericRepository<User>(_context);
            }
        }

        public IGenericRepository<Voucher> VoucherRepository
        {
            get
            {
                return voucherRepository ??= new GenericRepository<Voucher>(_context);
            }
        }
        public IGenericRepository<TopUpRequest> TopUpRequestRepository
        {
            get
            {
                return topUpRequestRepository ??= new GenericRepository<TopUpRequest>(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
