using AutoMapper;
using ECom.Core.Entites;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrasructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Repositories
{
    public class UintOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailServices emailServices;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenrationToken token;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasket { get; }

        public IAuth Auth { get; }

        public UintOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,
            IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailServices emailServices, SignInManager<AppUser> signInManager, IGenrationToken token)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            this.redis = redis;
            this.userManager = userManager;
            this.emailServices = emailServices;
            this.signInManager = signInManager;
            this.token = token;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context, _mapper, _imageManagementService);
            PhotoRepository = new PhotoRepository(context);
            CustomerBasket = new CustomerBasketRepository(redis);
            Auth = new AuthRepository(userManager, emailServices, signInManager,token, context);
            
        }
    }
}
