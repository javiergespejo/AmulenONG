﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABM.Models;

namespace ABM.Repository
{
    public class UnitOfWork : IDisposable
    {
        private AmulenEntities context = new AmulenEntities();
        private GenericRepository<ImportantFile> fileRepository;
        private GenericRepository<HomePageImage> homePageImageRepository;
        private GenericRepository<HomePageData> homePageDataRepository;
        private GenericRepository<Proyect> projectRepository;
        private UserRepository userRepository;
        private AdminRepository adminRepository;

        public GenericRepository<ImportantFile> FileRepository
        {
            get
            {

                if (this.fileRepository == null)
                {
                    this.fileRepository = new GenericRepository<ImportantFile>(context);
                }
                return fileRepository;
            }
        }
        public GenericRepository<HomePageImage> HomePageImageRepository
        {
            get
            {

                if (this.homePageImageRepository == null)
                {
                    this.homePageImageRepository = new GenericRepository<HomePageImage>(context);
                }
                return homePageImageRepository;
            }
        }
        public UserRepository UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }
        public AdminRepository AdminRepository 
        {
            get
            {
                if(this.adminRepository == null)
                {
                    this.adminRepository = new AdminRepository(context);
                }
                return adminRepository;
            }
        }
        public GenericRepository<HomePageData> HomePageDataRepository
        {
            get
            {

                if (this.homePageDataRepository == null)
                {
                    this.homePageDataRepository = new GenericRepository<HomePageData>(context);
                }
                return homePageDataRepository;
            }
        }
        public GenericRepository<Proyect> ProjectRepository
        {
            get
            {

                if (this.projectRepository == null)
                {
                    this.projectRepository = new GenericRepository<Proyect>(context);
                }
                return projectRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}