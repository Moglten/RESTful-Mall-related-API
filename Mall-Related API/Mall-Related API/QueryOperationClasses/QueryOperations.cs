using Mall_Related_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall_Related_API.Repository;
using System;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace Mall_Related_API.Classes
{
    public class QueryOperations<T> where T:class
    {

        //Generic API Query Operation ON
        //Sort Operation
        public IQueryable<T> SortingTheData(string orderBy, IQueryable<T> repository)
        {
            var order = orderBy.Split(" ");

            var orderWay = orderBy.Contains("desc") ? false : true;
            try
            {
                return repository
                    .OrderByPropertyName(order[0], orderWay);
            }
            catch
            {
                return null;
            }
        }

            

        //Filter Operation
        public IQueryable<T> FiltringTheData(string FilterBy, IQueryable<T> repository)
        {
            string[] filter = null;
            string filterOpertator = null;
            if (FilterBy.Contains("="))
            {
                filterOpertator = "=";
                filter = FilterBy.Split("=");
            } 
            else if (FilterBy.Contains(">=")) 
            {
                filterOpertator = ">=";
                filter = FilterBy.Split(">=");
            } 
            else if (FilterBy.Contains("<="))
            {
                filterOpertator = "<=";
                filter = FilterBy.Split("<=");
            }
            else if (FilterBy.Contains(">"))
            {
                filterOpertator = ">";
                filter = FilterBy.Split(">");
            }
            else
            {
                filterOpertator = "<";
                filter = FilterBy.Split("<");
            }


            filter[1] = filter[1].Replace(".","");
            try
                {
                    return repository
                            .FilterByPropertyName(filter[0], filter[1], filterOpertator);
                }
            catch
                {
                    return null;
                }
            }
        }
    }

