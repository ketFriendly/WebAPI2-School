using It_Girls_Projekat.Models;
using System;
using System.Data.Entity;
using Unity.Attributes;

namespace It_Girls_Projekat.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }
        //ovo brisano
        //[Dependency]
        //public IGenericRepository<ApplicationUser> UsersRepository { get; set; }

        [Dependency]
        public IAuthRepository AuthRepository { get; set; }

        [Dependency]
        public IGenericRepository<Admin> AdminsRepository { get; set; }

        [Dependency]
        public IStudentsRepository StudentsRepository { get; set; }

        [Dependency]
        public IParentsRepository ParentsRepository { get; set; }

        [Dependency]
        public ITeachersRepository TeachersRepository { get; set; }

        [Dependency]
        public IMarksRepository MarksRepository { get; set; }

        [Dependency]
        public ISubjectsRepository SubjectsRepository { get; set; }

        [Dependency]
        public IClassesRepository ClassesRepository { get; set; }

        [Dependency]
        public IGenericRepository<TeachesSubjects> teach_subj_repo { get; set; }

        //[Dependency]
        //public IGenericRepository<TeachesSubjectsClasses> {get; set;}
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