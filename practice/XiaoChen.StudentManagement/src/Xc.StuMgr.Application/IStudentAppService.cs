

using System.Collections.Generic;
using Xc.StuMgr.Application.Dtos;

namespace Xc.StuMgr.Application
{
    public interface IStudentAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentOutput Get(int id);

        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        List<StudentOutput> Get();

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="input"></param>
        void Update(UpdateStudentInput input);

        /// <summary>
        /// 更新学生年龄
        /// </summary>
        /// <param name="age"></param>
        void UpdateAge(int age);

        /// <summary>
        /// 根据ID删除学生
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="input"></param>
        void Create(CreateStudentInput input);
    }
}