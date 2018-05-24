using System;
using CatsProj.DB;
using SqlSugar;

namespace DBHelper
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			SqlSugarClient instance = SqlSugarInstance.newInstance();
			instance.DbFirst.CreateClassFile("Models");
        }
    }
}
