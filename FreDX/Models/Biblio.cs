using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreDX.Models
{
    //----------------Информация связанная с поступление заявки----------------------------

    public class Enterbiblio
    {
        public int? Id { get; set; }
        public DateTime DataInv { get; set; }
        public string LangIn { get; set; }
        public string LangTr { get; set; }
        public string AgNum { get; set; }
        public DateTime DateEn { get; set; }
        public string Numerator { get; set; }
    }

    //----------------Название изобретения------------------------------------------------
    public class Inventionbiblio
    {
        public int Id { get; set; }
        public string Invname { get; set; }
        public string Department { get; set; }
        public string Year { get; set; }
        public string Numerator { get; set; }


    }

    //-----------------Информация о заявителях--------------------------------------------

    public class Appl
    {
        public int Id { get; set; }
        public string Applicant { get; set; }
        public string ResidentCountry { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Numerator { get; set; }

    }

    //----------------Информация об авторах--------------------------------------------------

    public class Invent
    {
        public int Id { get; set; }
        public string Inventor { get; set; }
        public string ResidentCountry { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Numerator { get; set; }

    }

    //----------------Информация об агентах--------------------------------------------------

    public class Agent
    {
        public int Id { get; set; }
        public string AgentNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Numerator { get; set; }

    }

    //-------------Информация о приоритетных документах---------------------------------------

    public class Priority
    {
        public int Id { get; set; }
        public string IdClaim { get; set; }
        public DateTime DataInv { get; set; }
        public string Department { get; set; }
        public string Country { get; set; }
        public string Numerator { get; set; }

    }

    public class Invention
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Numerator { get; set; }
    }
    public class FPath
    {
        public int Id { get; set; }
        public string Numerator { get; set; }
        public string FolderPath { get; set; }
    }
}