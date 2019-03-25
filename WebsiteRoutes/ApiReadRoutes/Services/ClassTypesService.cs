using System;
using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Services
{
    public class ClassTypesService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public ClassTypesService(int clubid, int? conceptid, int? language)
        {
            string query = "";

            if (language == 0 || language == 2)
            {
                query = @"select DISTINCT
                                    ClassTypes.ClassTypeId id, 
                                    IFNULL(ClassTypes.ClassType, '') name,
                                    ClassTypes.ConceptId conceptId,
                                    ClassCategories.CategoryName className,
                                    IFNULL(ClassCategories.Description, '') description
                            from Data_Layer_Test.ClassTypes
                            inner join Data_Layer_Test.ClassCategories on ClassCategories.ClassTypeId = ClassTypes.ClassTypeId and ClassCategories.ClassCategoryId = ClassTypes.CSIServiceId
                            where ClassCategories.EventFlag = false
                              and ClassCategories.ClubId = " + clubid.ToString();
            }
            else
            {
                query = @"select DISTINCT
                                    ClassTypes.ClassTypeId id, 
                                    IFNULL(ClassTypes.FrenchClassType, '') name,
                                    ClassTypes.ConceptId conceptId,
                                    IFNULL(ClassCategories.FrenchCategoryName, '') className,
                                    IFNULL(ClassCategories.FrenchDescription, '') description
                            from Data_Layer_Test.ClassTypes
                            inner join Data_Layer_Test.ClassCategories on ClassCategories.ClassTypeId = ClassTypes.ClassTypeId and ClassCategories.ClassCategoryId = ClassTypes.CSIServiceId
                            where ClassCategories.EventFlag = false
                              and ClassCategories.ClubId = " + clubid.ToString();
            }
            

            if(conceptid != null)
            {
                query = query + " and ClassTypes.ConceptId = " + conceptid.ToString();
            }

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public List<ClassTypes> GetClassTypes()
        {
            List<ClassTypes> classTypes = new List<ClassTypes>();

            BigQueryResults res = Results[0];

            foreach (BigQueryRow row in res)
            {
                ClassTypes types = new ClassTypes
                {
                    id = Convert.ToInt32(row["id"]),
                    classType = row["name"].ToString(),
                    conceptId = Convert.ToInt32(row["conceptId"]),
                    className = (row["className"]).ToString(),
                    description = row["description"].ToString(),
                };

                classTypes.Add(types);
            }

            return classTypes;
        }
    }
}
