

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Xc.StuMgr.Application.Dtos;

namespace Xc.StuMgr.Application
{
    public class StudentAppService: IStudentAppService
    {
        /// <summary>
        /// 根据ID获取学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public StudentOutput Get(int id)
        {
            return new StudentOutput() {Id = 1, Age = 18, Name = "张三"};
        }

        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        public List<StudentOutput> Get()
        {
            return new List<StudentOutput>()
            {
                new StudentOutput(){Id = 1,Age = 18,Name = "张三"},
                new StudentOutput(){Id = 2,Age = 19,Name = "李四"}
            };
        }

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="input"></param>
        public void Update(UpdateStudentInput input)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 更新学生年龄
        /// </summary>
        /// <param name="age"></param>
        [HttpPatch("{id:int}/age")]
        public void UpdateAge(int age)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 根据ID删除学生
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="input"></param>
        public void Create(CreateStudentInput input)
        {
            throw new System.NotImplementedException();
        }
    }
}