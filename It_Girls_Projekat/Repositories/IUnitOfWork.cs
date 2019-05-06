using It_Girls_Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_Girls_Projekat.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepository { get; }
        
        IGenericRepository<Admin> AdminsRepository { get; set; }

        IStudentsRepository StudentsRepository { get; set; }
        IParentsRepository ParentsRepository { get; set; }
        ITeachersRepository TeachersRepository { get; set; }
        IMarksRepository MarksRepository { get; set; }
        ISubjectsRepository SubjectsRepository { get; set; }
        IClassesRepository ClassesRepository { get; set; }
        IGenericRepository<TeachesSubjects> teach_subj_repo { get; set; }
        void Save();
    }
}
