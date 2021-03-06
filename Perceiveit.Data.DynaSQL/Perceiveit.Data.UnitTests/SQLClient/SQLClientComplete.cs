﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using Perceiveit.Data.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perceiveit.Data.UnitTests.SQLClient
{
    [TestClass]
    public class SQLClientComplete
    {

        #region public DBDatabase Database {get;}

        private DBDatabase _database;

        /// <summary>
        /// Gets the database reference for these tests
        /// </summary>
        public DBDatabase Database
        {
            get { return _database; }
        }

        #endregion

        #region public TestContext TestContext {get;set;}

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        //
        // Set up
        //

        #region public void InitDbs() + AttachTestContextProfiler()

        
        /// <summary>
        /// Sets up all the database connections that should be executed against
        /// </summary>
        [TestInitialize()]
        public void InitDbs()
        {
            //Modify the connections to suit
            //Comment any databases that should not be executed against

            DBDatabase sqlclient = DBDatabase.Create("SQLClient"
                                                    , SQLClient.Schools.DbConnection
                                                    , SQLClient.Schools.DbProvider);
            AttachTestContextProfiler(sqlclient);
            this._database = sqlclient;
            this._database.HandleException += new DBExceptionHandler(_database_HandleException);

            //DBDatabase mysql = DBDatabase.Create("mySql"
            //                                    , "server=172.16.56.1;User Id=testaccount;Password=test;Persist Security Info=True;database=northwind"
            //                                    , "MySql.Data.MySqlClient");
            //AttachTestContextProfiler(mysql);

            //DBDatabase sqlite = DBDatabase.Create("MSAccess"
            //                                    , @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Sample Databases\Northwind2007.accdb;Persist Security Info=False;"
            //                                    , "System.Data.OleDb");
            //AttachTestContextProfiler(sqlite);

            //DBDatabase oracle = DBDatabase.Create("Oracle"
            //                                     , "DATA SOURCE=127.0.0.1;PERSIST SECURITY INFO=True;USER ID=DBUSER;PASSWORD=Test"
            //                                     , "Oracle.DataAccess.Client");
            //AttachTestContextProfiler(oracle);


           
        }

        void _database_HandleException(object sender, DBExceptionEventArgs args)
        {
            TestContext.WriteLine("Database encountered an error : {0}", args.Message);
            //Not nescessary - but hey, validates it's writable.
            args.Handled = false;
        }

        /// <summary>
        /// Attaches a profiler to the database connection.
        /// Reports the sql execution of all statements against the database to the TestContext
        /// </summary>
        /// <param name="database"></param>
        private void AttachTestContextProfiler(DBDatabase database)
        {
            TestContextProfiler profiler = new TestContextProfiler(database.Name, this.TestContext);
            database.AttachProfiler(profiler, true);
        }

        #endregion

        //
        // Test Method
        //

        #region public void SQLClient_01_CreateV1Tables()

        /// <summary>
        /// Create all the Version 1 data tables using the SQLCLient Schools schema
        /// </summary>
        [TestMethod()]
        public void SQLClient_01_CreateV1Tables()
        {
            DBDatabase db = this.Database;


            DBQuery create;

            //Create Departments
            create = DBQuery.Create.Table(Schools.Schema, Schools.Departments.Table)
                                           .Add(Schools.Departments.Id)
                                           .Add(Schools.Departments.Name)
                                           .Add(Schools.Departments.DateCreated)
                                           .Add(Schools.Departments.LastModified)
                                           .Add(Schools.Departments.Budget)
                                           .Add(Schools.Departments.Administrator);
            db.ExecuteNonQuery(create);

            //Create Courses
            create = DBQuery.Create.Table(Schools.Schema, Schools.Courses.Table)
                                    .Add(Schools.Courses.Id)
                                    .Add(Schools.Courses.Title)
                                    .Add(Schools.Courses.Credits)
                                    .Add(Schools.Courses.Department)
                                    .Add(Schools.Courses.Description)
                                    .Constraints(
                                        DBConstraint.ForeignKey().Column(Schools.Courses.Department)
                                                    .References(Schools.Departments.Table).Column(Schools.Departments.Id));
            db.ExecuteNonQuery(create);

            

            //Create Onsite Courses with an FK onto COURSES (cascade delete)
            create = DBQuery.Create.Table(Schools.Schema, Schools.OnsiteCourse.Table)
                                   .Add(Schools.OnsiteCourse.Id)
                                   .Add(Schools.OnsiteCourse.Location)
                                   .Add(Schools.OnsiteCourse.Days)
                                   .Add(Schools.OnsiteCourse.Time)
                                   .Constraints(
                                        DBConstraint.ForeignKey().Column(Schools.OnsiteCourse.Id)
                                                        .References(Schools.Courses.Table).Column(Schools.Courses.Id)
                                                        .OnDelete(DBFKAction.Cascade));
            db.ExecuteNonQuery(create);

            //Create Online Courses with a FK onto COURSES (cascade delete)
            create = DBQuery.Create.Table(Schools.Schema, Schools.OnlineCourse.Table)
                                   .Add(Schools.OnlineCourse.Id)
                                   .Add(Schools.OnlineCourse.Url)
                                   .Constraints(
                                        DBConstraint.ForeignKey().Column(Schools.OnlineCourse.Id)
                                                        .References(Schools.Courses.Table).Column(Schools.Courses.Id)
                                                        .OnDelete(DBFKAction.Cascade));
            db.ExecuteNonQuery(create);

            //Create Persons
            create = DBQuery.Create.Table(Schools.Schema, Schools.Person.Table)
                                   .Add(Schools.Person.Id)
                                   .Add(Schools.Person.First)
                                   .Add(Schools.Person.Last)
                                   .Add(Schools.Person.HireDate)
                                   .Add(Schools.Person.EnrollmentDate);
            db.ExecuteNonQuery(create);

            //Create Instructors
            create = DBQuery.Create.Table(Schools.Schema, Schools.Instructor.Table)
                                   .Add(Schools.Instructor.CourseID)
                                   .Add(Schools.Instructor.PersonID)
                                   .Constraints(
                                        DBConstraint.PrimaryKey().Columns(Schools.Instructor.CourseID, Schools.Instructor.PersonID),
                                        DBConstraint.ForeignKey().Column(Schools.Instructor.CourseID)
                                                        .References(Schools.Courses.Table).Column(Schools.Courses.Id),
                                        DBConstraint.ForeignKey().Column(Schools.Instructor.PersonID)
                                                        .References(Schools.Person.Table).Column(Schools.Person.Id));
            db.ExecuteNonQuery(create);

        }

        #endregion

        
        //
        // insert data from the SampleData files
        //

        #region public void SQLClient_02_InsertDepartmentData()


        /// <summary>
        /// Loads all the Departments from a simple CSV file and inserts 
        /// them into the Departments data table using delegate parameters
        /// </summary>
        [TestMethod()]
        public void SQLClient_02_InsertDepartmentData()
        {
            DBDatabase db = this.Database;

            //first check the original count on the database for comparison later
            DBQuery getcount = DBQuery.SelectCount().From(Schools.Schema, Schools.Departments.Table);
            int origcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            TestContext.WriteLine("Total number of departments before inserts = {0}", origcount);


            //Load Departments
            CSVData depts = CSVData.ParseString(SampleData.Departments, true);
            CSVItem[] all = depts.Items;
            CSVItem item = null;

            //Get the column offsets from the CSV file for name and budget - administrator comes later
            int nameoffset = depts.GetOffset("Name");
            int budgetoffset = depts.GetOffset("Budget");
            
            //Build parameters for retrieving the data at the offsets using anonymous delegates
            DBParam pname = DBParam.ParamWithDelegate(DbType.String, delegate { return item[nameoffset]; });
            DBParam pbudget = DBParam.ParamWithDelegate(DbType.Currency, delegate { return double.Parse(item[budgetoffset]); });
            

            //Insert query using fields onto parameters
            DBQuery ins = DBQuery.InsertInto(Schools.Schema, Schools.Departments.Table)
                                 .Fields(Schools.Departments.Name.Name, Schools.Departments.Budget.Name)
                                 .Values(pname, pbudget);


            //Wrap it all in a transaction
            using (DbTransaction trans = db.BeginTransaction())
            {
                int sum = 0;
                for (int i = 0; i < all.Length; i++)
                {
                    item = all[i];
                    sum += db.ExecuteNonQuery(trans, ins);
                }

                trans.Commit();
                TestContext.WriteLine("Number of inserts = {0}", sum);
            }


            //Validate that they have all been inserted
            int newcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            Assert.AreEqual(origcount + all.Length, newcount);
            TestContext.WriteLine("Total number of departments after inserts = {0}", newcount);
        }

        #endregion

        #region public void SQLClient_03_InsertCourseData()

        [TestMethod()]
        public void SQLClient_03_InsertCourseData()
        {

            DBDatabase db = this.Database;

            //first check the original count on the database for comparison later
            DBQuery getcount = DBQuery.SelectCount().From(Schools.Schema, Schools.Courses.Table);
            int origcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            TestContext.WriteLine("Total number of cousrses before inserts = {0}", origcount);


            //Load Departments
            CSVData course = CSVData.ParseString(SampleData.Courses, true);
            CSVItem[] all = course.Items;
            

            //Get the column offsets from the CSV file.
            int idoffset = course.GetOffset("ID");
            int titleoffset = course.GetOffset("Title");
            int credsoffset = course.GetOffset("Credits");
            int deptoffset = course.GetOffset("Department");
            

 
            //Different stategy here - do an insert into the courses based on a lookup against department
            //                         with the other parameters as values on that select

            DBParam pid = DBParam.ParamWithValue("id", Schools.Courses.Id.Type, null);
            DBParam ptitle = DBParam.ParamWithValue("title", Schools.Courses.Title.Type, Schools.Courses.Title.Length, null);
            DBParam pcreds = DBParam.ParamWithValue("creds", Schools.Courses.Credits.Type, null);

            DBParam pdept = DBParam.ParamWithValue("dept", DbType.String, null); //this is the name used to lookup the id - not the id itself


           
            DBQuery insert = DBQuery.InsertInto(Schools.Schema, Schools.Courses.Table)
                                    .Field(Schools.Courses.Id.Name)
                                    .Field(Schools.Courses.Title.Name)
                                    .Field(Schools.Courses.Credits.Name)
                                    .Field(Schools.Courses.Department.Name)
                                    .Select(
                                        DBQuery.Select(pid, ptitle, pcreds, DBField.Field(Schools.Departments.Id.Name))
                                                .From(Schools.Schema, Schools.Departments.Table)
                                                .WhereFieldEquals(Schools.Departments.Name.Name, pdept)
                                        );

            
            // Wrap it all in an exception again.
            using (DbTransaction trans = db.BeginTransaction())
            {
                int sum = 0;
                //now we update all the values on the parameters based on the current item
                foreach (CSVItem item in all)
                {
                    pid.Value = item[idoffset];
                    ptitle.Value = item[titleoffset];
                    pcreds.Value = Convert.ToSingle(item[credsoffset]);
                    pdept.Value = item[deptoffset];

                    //perform the execution
                    sum += Convert.ToInt32(db.ExecuteNonQuery(trans, insert));

                }
                trans.Commit();
                TestContext.WriteLine("Number of inserts = {0}", sum);
            }

            int newcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            Assert.AreEqual(origcount + all.Length, newcount);
            TestContext.WriteLine("Total number of courses after inserts = {0}", newcount);

        }

        #endregion

        #region public void SQLClient_04_InsertOnSiteCourseData()

        /// <summary>
        /// Simple quick inserts using Const values for a change. 
        /// Performs a transactional rollback first and then re-does with a commit
        /// </summary>
        [TestMethod()]
        public void SQLClient_04_InsertOnSiteCourseData()
        {
            DBDatabase db = this.Database;

            DBQuery getcount = DBQuery.SelectCount().From(Schools.Schema, Schools.OnsiteCourse.Table);
            int origcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            TestContext.WriteLine("Total number of onsite courses before inserts = {0}", origcount);

            //load the Onsite courses
            CSVData course = CSVData.ParseString(SampleData.OnsiteCourses, true);
            CSVItem[] all = course.Items;

            //Get the column offsets from the CSV file.
            int idoffset = course.GetOffset("ID");
            int locationoffset = course.GetOffset("Location");
            int daysoffset = course.GetOffset("Days");
            int timeoffset = course.GetOffset("Time");
            
            //create the constants with empty values
            DBConst id = DBConst.String("");
            DBConst loc = DBConst.String("");
            DBConst days = DBConst.Int32(0);
            DBConst time = DBConst.DateTime(DateTime.MinValue);

            DBQuery insert = DBQuery.InsertInto(Schools.Schema, Schools.OnsiteCourse.Table)
                                    .Fields(Schools.OnsiteCourse.Id.Name,
                                            Schools.OnsiteCourse.Location.Name,
                                            Schools.OnsiteCourse.Days.Name,
                                            Schools.OnsiteCourse.Time.Name)
                                    .Values(id, loc, days, time);

            using (DbTransaction trans = db.BeginTransaction())
            {
                foreach (CSVItem item in all)
                {
                    id.Value = item[idoffset];
                    loc.Value = item[locationoffset];
                    days.Value = Convert.ToInt32(item[daysoffset]);
                    time.Value = DateTime.ParseExact(item[timeoffset], "HH:mm:ss", null);

                    db.ExecuteNonQuery(trans, insert);
                }
                //Do not commit
            }
            int uncommitedCount = Convert.ToInt32(db.ExecuteScalar(getcount));
            Assert.AreEqual(origcount, uncommitedCount);
            TestContext.WriteLine("After uncommited inserts and transaction rollback the row count was still '{0}'", uncommitedCount);

            //repeat with a commit at the end
            using (DbTransaction trans = db.BeginTransaction())
            {
                foreach (CSVItem item in all)
                {
                    id.Value = item[idoffset];
                    loc.Value = item[locationoffset];
                    days.Value = Convert.ToInt32(item[daysoffset]);
                    time.Value = DateTime.ParseExact(item[timeoffset], "HH:mm:ss", null);

                    db.ExecuteNonQuery(trans, insert);
                }
                //Now commit
                trans.Commit();
            }

            int newCount = Convert.ToInt32(db.ExecuteScalar(getcount));
            Assert.AreEqual(origcount + all.Length, newCount);
            TestContext.WriteLine("After {0} committed inserts new count is {1}", all.Length, newCount);

        }

        #endregion

        #region public void SQLClient_05_InsertOnlineCourseData()

        /// <summary>
        /// Non transactional inserts into the database for all the online courses
        /// </summary>
        [TestMethod()]
        public void SQLClient_05_InsertOnlineCourseData()
        {
            DBDatabase db = this.Database;

            DBQuery getcount = DBQuery.SelectCount().From(Schools.Schema, Schools.OnlineCourse.Table);
            int origcount = Convert.ToInt32(db.ExecuteScalar(getcount));
            TestContext.WriteLine("Total number of online courses before inserts = {0}", origcount);

            CSVData data = CSVData.ParseString(SampleData.OnlineCourses, true);
            CSVItem[] all = data.Items;
            CSVItem item = null;

            int cidoffset = data.GetOffset("Id");
            int urloffset = data.GetOffset("Url");

            DBParam id = DBParam.ParamWithDelegate(delegate { return item[cidoffset]; });
            DBParam url = DBParam.ParamWithDelegate(delegate { return item[urloffset]; });

            //Test with the natural ordering of the columns
            DBQuery insert = DBQuery.InsertInto(Schools.Schema, Schools.OnlineCourse.Table)
                                    .Values(id, url);

            //Don't lock this in a transation - to ensure it is committed after each round.
            for (int i = 0; i < all.Length; i++)
            {
                item = all[i];
                db.ExecuteNonQuery(insert);

                //now check that it has been inserted.
                int count = Convert.ToInt32(db.ExecuteScalar(getcount));
                Assert.AreEqual(origcount + i + 1, count); //we add one because of the position in the loop
                TestContext.WriteLine("Inserted an online course and count is now {0}", count);
            }
        }

        #endregion

        #region public void SQLClient_06_InsertPeopleData()

        /// <summary>
        /// Inserts all the people into the table with 
        /// NULL checking for the Hire and Enrollment dates
        /// </summary>
        [TestMethod()]
        public void SQLClient_06_InsertPeopleData()
        {
            DBDatabase db = this.Database;

            CSVData data = CSVData.ParseString(SampleData.People, true);
            CSVItem[] all = data.Items;
            CSVItem item = null;

            int firstOffset = data.GetOffset("First");
            int lastOffset = data.GetOffset("Last");
            int hireOffset = data.GetOffset("HireDate");
            int enrollOffset = data.GetOffset("Enrollment");

            //Lets do some null checking on these 
            DBParam first = DBParam.ParamWithDelegate(DbType.String, delegate
            {
                return item[firstOffset];
            });

            DBParam second = DBParam.ParamWithDelegate(DbType.String, delegate
            {
                return item[lastOffset];
            });

            DBParam hire = DBParam.ParamWithDelegate(DbType.DateTime2, delegate
            {
                if (item.IsNull(hireOffset))
                    return DBNull.Value;
                else
                    return DateTime.Parse(item[hireOffset]);
            });

            DBParam enroll = DBParam.ParamWithDelegate(DbType.DateTime2, delegate
            {
                if (item.IsNull(enrollOffset))
                    return DBNull.Value;
                else
                    return DateTime.Parse(item[enrollOffset]);
            });

            DBQuery insert = DBQuery.InsertInto(Schools.Schema, Schools.Person.Table)
                                    .Fields(Schools.Person.First.Name,
                                            Schools.Person.Last.Name,
                                            Schools.Person.HireDate.Name,
                                            Schools.Person.EnrollmentDate.Name)
                                    .Values(first, second, hire, enroll);

            for (int i = 0; i < all.Length; i++)
            {
                item = all[i];
                //Do the insert and check that 1 was inserted
                Assert.AreEqual(1, db.ExecuteNonQuery(insert));
            }
        }

        #endregion

        #region public void SQLClient_07_InsertInstructors()

        /// <summary>
        /// Looks up the IDs of the People with a string concatenation and inserts the id 
        /// along with the course into the instructors table.
        /// This handles fake rows in the data for both unknown course ID's and unknown people
        /// </summary>
        [TestMethod()]
        public void SQLClient_07_InsertInstructors()
        {
            DBDatabase db = this.Database;

            CSVData instructors = CSVData.ParseString(SampleData.Instructors, true);
            CSVItem[] all = instructors.Items;
            CSVItem item = null;

            int courseOffset = instructors.GetOffset("Course");
            int personOffset = instructors.GetOffset("Instructor");
            int fakeOffset = instructors.GetOffset("Fake"); // a flag column for fake entries.

            string firstCol = Schools.Person.First.Name;
            string lastCol = Schools.Person.Last.Name;
            string pidCol = Schools.Person.Id.Name;

            //Paramter onto the course id of the current item.
            DBParam cid = DBParam.ParamWithDelegate("courseid", delegate { return item[courseOffset]; });

            //Parameter onto the full name of the person
            DBParam pname = DBParam.ParamWithDelegate("name", delegate { return item[personOffset]; });


            //We run an insert as a sub select. We don't know the autonumber Id of the 
            //person from the csv file, but we do know the full name.
            //The data is stored as FirstName and LastName so we can run a concatentation
            //in the where clause to compare it to the provided name
            // WHERE ([FirstName] + " " + [LastName] LIKE @name)
            
            DBQuery insert = DBQuery.InsertInto(Schools.Schema, Schools.Instructor.Table)
                                    .Fields(Schools.Instructor.CourseID.Name, Schools.Instructor.PersonID.Name)
                                    .Select(
                                        DBQuery.Select(cid, DBField.Field(pidCol))
                                               .From(Schools.Schema, Schools.Person.Table)
                                               .Where(DBField.Field(firstCol) + DBConst.String(" ") + DBField.Field(lastCol), Compare.Like, pname)
                                               );

            for (int i = 0; i < all.Length; i++)
            {
                item = all[i];
                bool isfake = item[fakeOffset] == "True";

                int inserted = 0;
                try
                {
                    TestContext.WriteLine("Inserting {0}, {1} ", item[courseOffset], item[personOffset]);
                    inserted = db.ExecuteNonQuery(insert);

                    if (isfake)
                        Assert.AreEqual(0, inserted);
                    else
                        Assert.AreEqual(1, inserted);
                }
                catch (Exception ex)
                {
                    //If we are a fake row then this is OK, otherwise rethrow
                    if (!isfake)
                        throw;
                }
                //Validate that one was inserted as we don't allow invalid names
                
            }
        }

        #endregion


        //
        // indexes
        //

        #region public void SQLClient_08_CreateIndexes()

        private const string DeptNameIndex = "DSQL_idx_DEPARTMENT_name";
        private const string CourseTitleIndex = "DSQL_idx_COURSE_title";

        /// <summary>
        /// Creates indexes on the Departments and courses tables
        /// </summary>
        [TestMethod()]
        public void SQLClient_08_CreateIndexes()
        {
            DBDatabase db = this.Database;

            //Create unique index on Department name if it doesn't exist
            DBQuery create = DBQuery.Create.Index(DeptNameIndex).Unique().NonClustered().On(Schools.Schema, Schools.Departments.Table)
                                .Columns(Schools.Departments.Name.Name);
                                //SQLClient implementation does not support the IfNotExisits
            db.ExecuteNonQuery(create);
            TestContext.WriteLine("Successfully executed the create index");


            create = DBQuery.Create.Index(CourseTitleIndex).Unique().NonClustered()
                                    .On(Schools.Courses.Table)
                                    .Columns(Schools.Courses.Title.Name);
            db.ExecuteNonQuery(create);

            //Re-run the create WITHOUT the if not exists.
            //Should throw an error.
            bool failed;
            try
            {
                db.ExecuteNonQuery(create);
                failed = false;
            }
            catch (Exception ex)
            {
                failed = true;
                TestContext.WriteLine("Successfully caught the duplicate exception :{0}", ex);
            }

            if (!failed)
                throw new InvalidOperationException("The create should have thrown an exception when trying to create again");

        }

        #endregion

        //
        // update statements
        //

        #region public void SQLClient_09_UpdateCourseAdministrators()

        /// <summary>
        /// Runs a number of statements against the department administrators
        /// to look them up in the people table and udate the value in the Departments table
        /// </summary>
        [TestMethod()]
        public void SQLClient_09_UpdateCourseAdministrators()
        {
            DBDatabase db = this.Database;

            CSVData depts = CSVData.ParseString(SampleData.Departments, true);
            int deptnameOffset = depts.GetOffset("Name");
            int deptadminOffset=  depts.GetOffset("Admin");

            DBParam pDeptID = DBParam.Param("deptid",DbType.Int32);
            DBParam pDeptName = DBParam.Param("deptName",DbType.String,100);
            DBParam pAdminID = DBParam.Param("adminid",DbType.Int32);
            DBParam pAdminName = DBParam.Param("adminName", DbType.String,100);

            //Want to do this in a script to test the Declaration and Set actions.
            DBQuery upd = DBQuery.Script()
                //declare the variables to hold the id values
                .Declare(pDeptID)
                .Declare(pAdminID)

                //Look up the deparment id based on it's name
                .Append(DBQuery.SelectTopN(1)
                               .Field(DBAssign.Set(pDeptID, DBField.Field(Schools.Departments.Id.Name)))
                               .From(Schools.Departments.Table)
                               .WhereFieldEquals(Schools.Departments.Name.Name, pDeptName))

                //Look up the Person id based on their concatenated name
                .Append(DBQuery.SelectTopN(1)
                               .Field(DBAssign.Set(pAdminID, DBField.Field(Schools.Person.Id.Name)))
                               .From(Schools.Schema, Schools.Person.Table)
                               .Where(DBField.Field(Schools.Person.First.Name) + DBConst.String(" ") + DBField.Field(Schools.Person.Last.Name), Compare.Equals, pAdminName))
                
                //Update the Admin ID for the Department ID
                .Append(DBQuery.Update(Schools.Schema, Schools.Departments.Table)
                                .Set(Schools.Departments.Administrator.Name, pAdminID)
                                .WhereField(Schools.Departments.Id.Name, Compare.Equals, pDeptID)
                );


            foreach (CSVItem item in depts.Items)
            {
                pDeptName.Value = item[deptnameOffset];
                pAdminName.Value = item[deptadminOffset];

                db.ExecuteNonQuery(upd);
                
            }

            //Validation - select all the departments still with a null administrator
            DBQuery selectNulls = DBQuery.SelectFields(Schools.Departments.Id.Name, Schools.Departments.Name.Name)
                                         .From(Schools.Schema, Schools.Departments.Table)
                                         .WhereField(Schools.Departments.Administrator.Name, Compare.Is, DBConst.Null());
            
            List<string> unmatched = new List<string>();

            db.ExecuteRead(selectNulls, reader =>
            {
                while (reader.Read())
                {
                    unmatched.Add(reader.GetString(1));// Department Name.
                }
            });

            if (unmatched.Count > 0)
                TestContext.WriteLine("The following departments did not have matching Administrators '{0}'", string.Join(", ", unmatched.ToArray()));
            else
                TestContext.WriteLine("All departments had a matching Administrator");
        }

        #endregion

        //
        // create views and stored procedures
        //

        #region public void SQLClient_10_CreateCourseViews()

        private const string OnlyOnlineCoursesView = "DSQL_ONLY_ONLINE_COURSES";
        private const string OnlyOnSiteCoursesView = "DSQL_ONLY_ONSITE_COURSES";

        /// <summary>
        /// Creates 2 views for only online courses and only on-site courses and checks that they
        /// return the correct data. Make sure the account you are using has these permissions.
        /// </summary>
        [TestMethod()]
        public void SQLClient_10_CreateCourseViews()
        {
            DBDatabase db = this.Database;

            DBQuery view = DBQuery.Create.View(Schools.Schema, OnlyOnlineCoursesView).As(
                DBQuery.Select()
                            .Field("C", Schools.Courses.Id.Name)
                            .Field("C", Schools.Courses.Title.Name)
                            .Field("C", Schools.Courses.Credits.Name)
                            .Field("C", Schools.Courses.Department.Name)
                            .Field("C", Schools.Courses.Description.Name)
                            .Field("OC", Schools.OnlineCourse.Url.Name)
                        .From(Schools.Schema, Schools.Courses.Table).As("C")
                            .InnerJoin(Schools.Schema, Schools.OnlineCourse.Table).As("OC")
                            .On("C", Schools.Courses.Id.Name, Compare.Equals, "OC", Schools.OnlineCourse.Id.Name)
                            );

            db.ExecuteNonQuery(view);
            TestContext.WriteLine("The view '" + OnlyOnlineCoursesView + "' was created into the schema");


            //Validate that the view exists and the number of rows returned
            //is the same for the OnlineCourses table and the new view.

            DBQuery selectview = DBQuery.SelectCount().From(Schools.Schema, OnlyOnlineCoursesView);
            int viewcount = Convert.ToInt32(db.ExecuteScalar(selectview));

            DBQuery selecttble = DBQuery.SelectCount().From(Schools.Schema, Schools.OnlineCourse.Table);
            int tblcount = Convert.ToInt32(db.ExecuteScalar(selecttble));

            Assert.AreEqual(tblcount, viewcount);
            TestContext.WriteLine("Validated that {0} rows are in the OnlineCourses table and {0} rows were returned from the OnlyOnlineCourses view", tblcount, viewcount);

            view = DBQuery.Create.View(Schools.Schema, OnlyOnSiteCoursesView).As(
                DBQuery.Select()
                            .Field("C", Schools.Courses.Id.Name)
                            .Field("C", Schools.Courses.Title.Name)
                            .Field("C", Schools.Courses.Credits.Name)
                            .Field("C", Schools.Courses.Department.Name)
                            .Field("C", Schools.Courses.Description.Name)
                            .Field("OC", Schools.OnsiteCourse.Location.Name)
                            .Field("OC", Schools.OnsiteCourse.Days.Name)
                            .Field("OC", Schools.OnsiteCourse.Time.Name)
                        .From(Schools.Schema, Schools.Courses.Table).As("C")
                            .InnerJoin(Schools.Schema, Schools.OnsiteCourse.Table).As("OC")
                            .On("C", Schools.Courses.Id.Name, Compare.Equals, "OC", Schools.OnsiteCourse.Id.Name)
                            );
            db.ExecuteNonQuery(view);
            TestContext.WriteLine("The view '" + OnlyOnSiteCoursesView + "' was created into the schema");


            //Validate that the view exists and the number of rows returned
            //is the same for the OnSiteCourses table and the new view.

            selectview = DBQuery.SelectCount().From(Schools.Schema, OnlyOnSiteCoursesView);
            viewcount = Convert.ToInt32(db.ExecuteScalar(selectview));

            selecttble = DBQuery.SelectCount().From(Schools.Schema, Schools.OnsiteCourse.Table);
            tblcount = Convert.ToInt32(db.ExecuteScalar(selecttble));

            Assert.AreEqual(tblcount, viewcount);
            TestContext.WriteLine("Validated that {0} rows are in the OnSiteCourses table and {0} rows were returned from the OnlyOnSiteCourses view", tblcount, viewcount);


        }

        #endregion

        #region public void SQLClient_11_CreateOnsiteCourseSProc()

        public const string InsertOnSiteCourseSproc = "DSQL_sp_INSERT_ONSITE_COURSE";

        /// <summary>
        /// Creates a stored procedure for insert an on-site course
        /// </summary>
        [TestMethod()]
        public void SQLClient_11_CreateOnsiteCourseSProc()
        {
            DBDatabase db = this.Database;

            //Create the stored procedure parameters
            DBParam cid = DBParam.Param("courseid", DbType.AnsiStringFixedLength, 4);
            DBParam ctitle = DBParam.Param("coursetitle", DbType.String, 100);
            DBParam ccredits = DBParam.Param("coursecredits", DbType.Single, 20);
            DBParam cdept = DBParam.Param("coursedept", DbType.Int32);
            DBParam cdesc = DBParam.Param("coursedesc", DbType.String, 1000);
            DBParam ocloc = DBParam.Param("courselocation", DbType.String, 100);
            DBParam ocdays = DBParam.Param("coursedays", DbType.Int32);
            DBParam octime = DBParam.Param("coursetime", DbType.Time);

            //Create the procedure that inserts a row into courses and a row into OnsiteCourses

            DBQuery insOnSite = DBQuery.Create.StoredProcedure(Schools.Schema, InsertOnSiteCourseSproc)
                                       .WithParams(cid, ctitle, ccredits, cdept, cdesc, ocloc, ocdays, octime)
                                       .As(
                                           DBQuery.InsertInto(Schools.Schema, Schools.Courses.Table)
                                                  .Fields(Schools.Courses.Id.Name,
                                                          Schools.Courses.Title.Name,
                                                          Schools.Courses.Credits.Name,
                                                          Schools.Courses.Department.Name,
                                                          Schools.Courses.Description.Name)
                                                  .Values(cid, ctitle, ccredits, cdept, cdesc),
                                           DBQuery.InsertInto(Schools.Schema, Schools.OnsiteCourse.Table)
                                                  .Fields(Schools.OnsiteCourse.Id.Name,
                                                          Schools.OnsiteCourse.Location.Name,
                                                          Schools.OnsiteCourse.Days.Name,
                                                          Schools.OnsiteCourse.Time.Name)
                                                  .Values(cid, ocloc, ocdays, octime)
                                            );

            //Execute to perform the insert
            db.ExecuteNonQuery(insOnSite);


            //So now we must execute the procedure to validate that it works

            //Query to check the current row count
            DBQuery countCourses = DBQuery.SelectCount().From(Schools.Schema, Schools.Courses.Table);
            DBQuery countOnSite = DBQuery.SelectCount().From(Schools.Schema, Schools.OnsiteCourse.Table);
            DBQuery countOnline = DBQuery.SelectCount().From(Schools.Schema, Schools.OnlineCourse.Table);

            //Query to get the Department ID
            DBQuery getDeptID = DBQuery.SelectTopN(1).Field(Schools.Departments.Id.Name)
                                                .From(Schools.Schema, Schools.Departments.Table)
                                                .WhereFieldEquals(Schools.Departments.Name.Name, DBConst.String("IT"));

            int origCourses = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int origOnSite = Convert.ToInt32(db.ExecuteScalar(countOnSite));
            int deptid = Convert.ToInt32(db.ExecuteScalar(getDeptID));

            //Set values for the parameters - we can use the same instances as in the CREATE PROCEDURE
            cid.Value = "NEWC";
            ctitle.Value = "New Onsite Course";
            ccredits.Value = 20.0;
            cdept.Value = deptid;
            cdesc.Value = "This is a new sample course";
            ocloc.Value = "West Wing";
            ocdays.Value = 5;
            octime.Value = "11:00:00";

            //Execute the sproc
            DBQuery exec = DBQuery.Exec(InsertOnSiteCourseSproc).WithParams(cid, ctitle, ccredits, cdept, cdesc, ocloc, ocdays, octime);
            db.ExecuteNonQuery(exec);

            int newCourses = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int newOnSite = Convert.ToInt32(db.ExecuteScalar(countOnSite));

            Assert.AreEqual(origCourses + 1, newCourses);
            Assert.AreEqual(origOnSite + 1, newOnSite);
            TestContext.WriteLine("New rows were inserted for the Courses and OnSiteCourses using the {0} Stored procedure ", InsertOnSiteCourseSproc);
        }

        #endregion

        #region public void SQLClient_12_CreateOnlineCourseSproc()

        public const string InsertOnlineCourseSproc = "DSQL_sp_INSERT_ONLINE_COURSE";

        /// <summary>
        /// Creates a stored procedure that inserts a new row in the Courses
        /// table and the Online courses table.
        /// </summary>
        [TestMethod()]
        public void SQLClient_12_CreateOnlineCourseSproc()
        {
             DBDatabase db = this.Database;

            DBParam cid = DBParam.Param("courseid", DbType.AnsiStringFixedLength, 4);
            DBParam ctitle = DBParam.Param("coursetitle", DbType.String, 100);
            DBParam ccredits = DBParam.Param("coursecredits", DbType.Single, 20);
            DBParam cdept = DBParam.Param("coursedept", DbType.Int32);
            DBParam cdesc = DBParam.Param("coursedesc", DbType.String, 1000);
            DBParam ocurl = DBParam.Param("courseurl", DbType.String, 200);

            //Query to check the current row count
            DBQuery countCourses = DBQuery.SelectCount().From(Schools.Schema, Schools.Courses.Table);
            DBQuery countOnline = DBQuery.SelectCount().From(Schools.Schema, Schools.OnlineCourse.Table);
            int origCourses = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int origOnline = Convert.ToInt32(db.ExecuteScalar(countOnline));

            //Query to get the Department ID
            DBQuery getDeptID = DBQuery.SelectTopN(1).Field(Schools.Departments.Id.Name)
                                                .From(Schools.Schema, Schools.Departments.Table)
                                                .WhereFieldEquals(Schools.Departments.Name.Name, DBConst.String("IT"));

            //Define the Stored procedure that inserts a row into courses and a row into Online Courses
            DBQuery insOnline = DBQuery.Create.StoredProcedure(Schools.Schema, InsertOnlineCourseSproc)
                                        .WithParams(cid, ctitle, ccredits, cdept, cdesc, ocurl)
                                        .As(
                                        DBQuery.InsertInto(Schools.Schema, Schools.Courses.Table)
                                               .Field(Schools.Courses.Id.Name)
                                               .Field(Schools.Courses.Title.Name)
                                               .Field(Schools.Courses.Credits.Name)
                                               .Field(Schools.Courses.Department.Name)
                                               .Field(Schools.Courses.Description.Name)
                                               .Values(cid, ctitle, ccredits, cdept, cdesc),
                                        DBQuery.InsertInto(Schools.Schema, Schools.OnlineCourse.Table)
                                               .Field(Schools.OnlineCourse.Id.Name)
                                               .Field(Schools.OnlineCourse.Url.Name)
                                               .Values(cid, ocurl)
                                        );
            //Create the procedure in the database
            db.ExecuteNonQuery(insOnline);

            cid.Value = "NEWU";
            ctitle.Value = "New Online Course";
            ccredits.Value = 20.0;
            cdept.Value = Convert.ToInt32(db.ExecuteScalar(getDeptID));
            cdesc.Value = "This is a new online sample course";
            ocurl.Value = "http://myuniversity.com/courses/newonline";

            //Execute the sproc
            DBQuery exec = DBQuery.Exec(InsertOnlineCourseSproc).WithParams(cid, ctitle, ccredits, cdept, cdesc, ocurl);
            db.ExecuteNonQuery(exec);

            int newCourses = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int newOnline = Convert.ToInt32(db.ExecuteScalar(countOnline));

            Assert.AreEqual(origCourses + 1, newCourses);
            Assert.AreEqual(origOnline + 1, newOnline);
            TestContext.WriteLine("New rows were inserted for the Courses and OnLineCourses using the {0} Stored procedure", InsertOnlineCourseSproc);

        }

        #endregion

        //
        // a few CRUD operations on the schema
        //

        #region public void SQLClient_13_SelectDepartmentCourses()

        /// <summary>
        /// Selects all the courses in a department including the 
        /// </summary>
        [TestMethod()]
        public void SQLClient_13_SelectDepartmentCourses()
        {
            DBParam deptname = DBParam.Param("deptname", DbType.String, Schools.Departments.Name.Length);

            
            DBQuery sel = DBQuery.Select()
                                     .Field("D", Schools.Departments.Name.Name).As("dept_name")
                                     .Field("DAD", Schools.Person.First.Name).As("admin_first")
                                     .Field("DAD", Schools.Person.Last.Name).As("admin_last")
                                     .Field("C", Schools.Courses.Id.Name).As("course_id")
                                     .Field("C", Schools.Courses.Title.Name).As("course_title")
                                 .From(Schools.Schema, Schools.Courses.Table).As("C")
                                     .InnerJoin(Schools.Schema, Schools.Departments.Table).As("D")
                                                .On("C", Schools.Courses.Department.Name, Compare.Equals, "D", Schools.Departments.Id.Name)
                                     .InnerJoin(Schools.Schema, Schools.Person.Table).As("DAD")
                                                .On("D", Schools.Departments.Administrator.Name, Compare.Equals, "DAD", Schools.Person.Id.Name)
                                 .WhereField("D", Schools.Departments.Name.Name, Compare.Equals, deptname);

            DBDatabase db = this.Database;

            deptname.Value = "IT";
            int count = 0;

            //Execute for the IT department and enumerate through the courses.
            db.ExecuteRead(sel, reader =>
            {
                while (reader.Read())
                {
                    //Just check - department names should match the parameter value
                    Assert.AreEqual(deptname.Value, reader.GetString(0));
                    TestContext.WriteLine("Course title in {0} dept : {1}",
                                           reader.GetString(0), reader.GetString(4));
                    count++;
                }
            });
            TestContext.WriteLine("Total number of courses in the '{0}' department is '{1}'", deptname.Value, count);
                
        }

        #endregion

        #region public void SQLClient_14_DeleteAnOnsiteCourse()

        /// <summary>
        /// Deletes an onsite course that was created by the insert procedure.
        /// As the FK has a cascade delete this should also delete the ONSITE_COURSE related row
        /// </summary>
        [TestMethod()]
        public void SQLClient_14_DeleteAnOnsiteCourse()
        {
            DBConst cid = DBConst.String("NEWC"); //This is the ID of the course that was previously created

            DBQuery del = DBQuery.DeleteFrom(Schools.Courses.Table)
                                 .WhereFieldEquals(Schools.Courses.Id.Name, cid);

            DBDatabase db = this.Database;

            //Get the original counts.
            DBQuery countCourses = DBQuery.SelectCount().From(Schools.Schema, Schools.Courses.Table);
            DBQuery countOnSite = DBQuery.SelectCount().From(Schools.Schema, Schools.OnsiteCourse.Table);
            
            int ccount = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int oscount = Convert.ToInt32(db.ExecuteScalar(countOnSite));

            db.ExecuteNonQuery(del);

            int new_ccount = Convert.ToInt32(db.ExecuteScalar(countCourses));
            int new_oscount = Convert.ToInt32(db.ExecuteScalar(countOnSite));

            Assert.AreEqual(ccount - 1, new_ccount);
            Assert.AreEqual(oscount - 1, new_oscount);
            TestContext.WriteLine("Deleted one row from courses and the corresponding row was automatically deleted from the related table");
            TestContext.WriteLine("New course count is {0}", ccount);
        }

        #endregion

        //
        // schema interrogation
        //

        #region public void SQLClient_15_ValidateTableSchema()

        [TestMethod()]
        public void SQLClient_15_ValidateTableSchema()
        {
            DBDatabase db = this.Database;


            //List of all the schema.table names in upper case.
            List<string> all = new List<string>(new string[] {
                Schools.Schema.ToUpper() + "." + Schools.Courses.Table.ToUpper(), 
                Schools.Schema.ToUpper() + "." + Schools.Departments.Table.ToUpper(), 
                Schools.Schema.ToUpper() + "." + Schools.Instructor.Table.ToUpper(),
                Schools.Schema.ToUpper() + "." + Schools.OnlineCourse.Table.ToUpper(), 
                Schools.Schema.ToUpper() + "." + Schools.OnsiteCourse.Table.ToUpper(), 
                Schools.Schema.ToUpper() + "." + Schools.Person.Table.ToUpper()});

            //Load all the tables from the database provider
            Schema.DBSchemaProvider provider = db.GetSchemaProvider();
            IEnumerable<Schema.DBSchemaItemRef> tables = provider.GetAllTables();

            //Go through each of the tables returned and 
            //based on their schema.name remove them from 
            //the collection if they are in there.
            //At the end we should have an empty collection

            foreach (Schema.DBSchemaItemRef tableref in tables)
            {
                string name = tableref.Name.ToUpper();
                string owner = tableref.Schema.ToUpper();
                string full = owner + "." + name;

                if (all.Contains(full))
                    all.Remove(full);
            }

            Assert.AreEqual(0, all.Count, "Not all the tables were returned from the schema provider");
            TestContext.WriteLine("All created tables we found and returned from the SchemaProvider");

            Schema.DBSchemaTable table = provider.GetTable("", Schools.Schema, Schools.Courses.Table);
            this.ValidateTableColumns(table, Schools.Courses.Id, Schools.Courses.Credits, Schools.Courses.Department, Schools.Courses.Description, Schools.Courses.Title);

            provider.GetAllIndexs();
        }

        private void ValidateTableColumns(Schema.DBSchemaTable table, params DBColumn[] columns)
        {
            foreach (DBColumn col in columns)
            {
                Assert.IsTrue(table.Columns.Contains(col.Name), "The table does not contain the column '{0}'", col.Name);
                Schema.DBSchemaTableColumn schemaCol = table.Columns[col.Name];
                Assert.AreEqual(col.Type, schemaCol.DbType);
                if ((col.Flags & DBColumnFlags.PrimaryKey) > 0)
                    Assert.IsTrue(schemaCol.PrimaryKey);

                if (col.Length > 0 &&
                    (col.Type == DbType.AnsiString || col.Type == DbType.AnsiStringFixedLength || col.Type == DbType.String || col.Type == DbType.StringFixedLength))
                {
                    Assert.AreEqual(schemaCol.Size, col.Length);
                }
            }
        }

        #endregion

        #region public void SQLClient_16_ValidateViewSchema()


        [TestMethod()]
        public void SQLClient_16_ValidateViewSchema()
        {
            DBDatabase db = this.Database;
            Schema.DBSchemaProvider provider = db.GetSchemaProvider();


            IEnumerable<Schema.DBSchemaItemRef> views = provider.GetAllViews();

            List<string> known = new List<string>(new string[] { OnlyOnlineCoursesView, OnlyOnSiteCoursesView });
            foreach (Schema.DBSchemaItemRef viewref in views)
            {
                bool isknown = false;
                if (string.Equals(viewref.Schema, Schools.Schema, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(viewref.Name, OnlyOnSiteCoursesView, StringComparison.OrdinalIgnoreCase))
                {
                    known.Remove(OnlyOnSiteCoursesView);
                    isknown = true;
                }

                else if (string.Equals(viewref.Schema, Schools.Schema, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(viewref.Name, OnlyOnlineCoursesView, StringComparison.OrdinalIgnoreCase))
                {
                    known.Remove(OnlyOnlineCoursesView);
                    isknown = true;
                }

                if (isknown)
                {
                    Schema.DBSchemaView fullview = provider.GetView(viewref);
                    Assert.AreNotEqual(fullview.Columns.Count, 0);
                }
            }

            Assert.IsTrue(known.Count == 0, "Not all the known views were returned");

        }

        #endregion

        #region public void SQLClient_17_ValidateIndexSchema()

        [TestMethod()]
        public void SQLClient_17_ValidateIndexSchema()
        {
            DBDatabase db = this.Database;
            Schema.DBSchemaProvider provider = db.GetSchemaProvider();


            IEnumerable<Schema.DBSchemaItemRef> indexes = provider.GetAllIndexs();

            List<string> known = new List<string>(new string[] { CourseTitleIndex.ToUpper(), DeptNameIndex.ToUpper() });

            foreach (Schema.DBSchemaItemRef item in indexes)
            {
                if (known.IndexOf(item.Name.ToUpper()) >= 0 && string.Equals(item.Schema, Schools.Schema, StringComparison.OrdinalIgnoreCase))
                {
                    known.Remove(item.Name.ToUpper());

                    Schema.DBSchemaIndex idx = provider.GetIndex(item);
                    Assert.IsNotNull(idx, "No index was returned for the refrerence {0}", item);
                    Assert.AreNotEqual(idx.Columns.Count, 0);
                }
            }

            Assert.AreEqual(0, known.Count, "Not all known indexes were removed");

        }

        #endregion

        #region public void SQLClient_18_ValidateSprocSchema()

        [TestMethod()]
        public void SQLClient_18_ValidateSprocSchema()
        {
            DBDatabase db = this.Database;
            Schema.DBSchemaProvider provider = db.GetSchemaProvider();


            IEnumerable<Schema.DBSchemaItemRef> sprocs = provider.GetAllStoredProcedures();

            List<string> known = new List<string>(new string[] { InsertOnlineCourseSproc.ToUpper(), InsertOnSiteCourseSproc.ToUpper() });

            foreach (Schema.DBSchemaItemRef item in sprocs)
            {
                if (known.IndexOf(item.Name.ToUpper()) >= 0 && string.Equals(item.Schema, Schools.Schema, StringComparison.OrdinalIgnoreCase))
                {
                    known.Remove(item.Name.ToUpper());

                    Schema.DBSchemaSproc sproc = provider.GetProcedure(item);
                    Assert.IsNotNull(sproc, "No index was returned for the refrerence {0}", item);
                    Assert.AreNotEqual(sproc.Parameters.Count, 0);
                }
            }

            Assert.AreEqual(0, known.Count, "Not all the known Stored Procedures were found");

        }

        #endregion

        //
        // secondary database
        // 

        /// <summary>
        /// Uses a second database reference to create a table, populate with data from the original database. 
        /// Run updates and deletions. 
        /// And finally drop the table - checking that it was deleted.
        /// </summary>
        [TestMethod()]
        public void SQLClient_19_CloneToDifferentDatabase()
        {
            DBDatabase db = this.Database;

            //
            //Create the table on the other database - we specify the database using the USE statement in a script
            //

            DBUseQuery use = DBQuery.Use(OtherDatabase.DbName);
            DBCreateTableQuery create = DBQuery.Create.Table(OtherDatabase.OtherPerson.Table)
                                                       .Add(OtherDatabase.OtherPerson.Id)
                                                       .Add(OtherDatabase.OtherPerson.First)
                                                       .Add(OtherDatabase.OtherPerson.Last)
                                                       .Add(OtherDatabase.OtherPerson.EnrollmentDate);

            DBScript createScript = DBQuery.Script(use
                                            , create);
            db.ExecuteNonQuery(createScript);

            //
            // after creating make sure it has not been created on the original database
            //

            DBQuery wrongSelect = DBQuery.SelectCount().From(Schools.Catalog, string.Empty, OtherDatabase.OtherPerson.Table);
            try
            {
                db.ExecuteScalar(wrongSelect);
                throw new InvalidOperationException("There should have been an error here - as the table should not exist on the schools catalog. One was not raised");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                TestContext.WriteLine("Table in other database successfully created");
            }


            //
            //Get the count of items in the Schools database.
            //

            DBQuery count = DBQuery.SelectCount().From(Schools.Catalog,Schools.Schema, Schools.Person.Table);
            int origNum = Convert.ToInt32(db.ExecuteScalar(count));
            if (origNum == 0)
                throw new InternalTestFailureException("Cannot continue - there are no item in the original table");
            else
                TestContext.WriteLine("There are " + origNum + " rows to migrate");

            //
            //push all items from the known table into the new table on the other databse.
            //

            DBInsertQuery ins = DBQuery.InsertInto(OtherDatabase.DbName, string.Empty, OtherDatabase.OtherPerson.Table)
                                        .Fields(OtherDatabase.OtherPerson.First.Name, OtherDatabase.OtherPerson.Last.Name, OtherDatabase.OtherPerson.EnrollmentDate.Name)
                                       .Select(
                                            //Inner select referring to the original database
                                            DBQuery.SelectFields(Schools.Person.First.Name, Schools.Person.Last.Name, Schools.Person.EnrollmentDate.Name)
                                                   .From(Schools.Catalog,Schools.Schema,Schools.Person.Table));
            db.ExecuteScalar(ins);

            //
            //run a check that all the items were inserted.
            //

            DBQuery newCount = DBQuery.SelectCount().From(OtherDatabase.DbName, string.Empty, OtherDatabase.OtherPerson.Table);
            int newNum = Convert.ToInt32(db.ExecuteScalar(newCount));
            Assert.AreEqual(newNum, origNum);
            TestContext.WriteLine("There are now " + newNum + " rows in the new database");

            //
            //Now run an update on the enrollment date of the other database
            //

            DBConst now = DBConst.DateTime(DateTime.Now);
            DBQuery update = DBQuery.Update(OtherDatabase.DbName, string.Empty, OtherDatabase.OtherPerson.Table)
                                    .Set(OtherDatabase.OtherPerson.EnrollmentDate.Name, now);
            int updated = db.ExecuteNonQuery(update);
            Assert.AreEqual(origNum, updated);
            TestContext.WriteLine("Updated " + updated + " rows to the current date time");

            //
            //No delete all the rows.
            //

            DBQuery del = DBQuery.DeleteFrom(OtherDatabase.DbName, string.Empty, OtherDatabase.OtherPerson.Table);
            int deleted = db.ExecuteNonQuery(del);
            Assert.AreEqual(origNum, deleted);
            TestContext.WriteLine("Deleted " + deleted + " rows from the other database table");

            //
            //Now just run a check to make sure that the orginal table was not altered
            //and that there a no longer any rows in the new table
            //

            Assert.AreEqual(origNum, Convert.ToInt32(db.ExecuteScalar(count)), "The number of rows in the original table has changed");
            TestContext.WriteLine("Row count on the original table remains the same");

            //
            //And then drop the other table with another script starting with use..
            //
            DBScript dropscript = DBQuery.Script(
                                    DBQuery.Use(OtherDatabase.DbName),
                                    DBQuery.Drop.Table(OtherDatabase.OtherPerson.Table).IfExists()
                                    );
            db.ExecuteNonQuery(dropscript);


            //After dropping run a check to make sure the table has been dropped.
            try
            {
                db.ExecuteScalar(newCount);
                throw new InvalidOperationException("There should have been an error here - as the table no longer exists. One was not raised");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                TestContext.WriteLine("Table in other database successfully dropped");
            }
        }

        // 
        // cleanup - Drop any schema elements that were created
        //

        [TestMethod()]
        public void SQLClient_21_CloneSchemaAndPopulate()
        {
            string tbl2Clone = "DQSL_COURSES";
            string cloneTbl = tbl2Clone + "_Clone";

            
            DBDatabase db = this.Database;

            //use the schema provider to extract the name and column info
            Schema.DBSchemaProvider sch = db.GetSchemaProvider();
            Schema.DBSchemaTable tbl = sch.GetTable(tbl2Clone);
            

            DBCreateTableQuery createClone = DBCreateTableQuery.Create.Table(tbl.Schema, cloneTbl);
            DBDropTableQuery dropClone = DBDropTableQuery.Drop.Table(tbl.Schema, cloneTbl).IfExists();

            DBInsertQuery ins = DBQuery.InsertInto(cloneTbl);
            DBSelectQuery sel = DBSelectQuery.Select();

            foreach (Schema.DBSchemaTableColumn col in tbl.Columns)
            {
                //append the columns to the create statement
                if(col.Size > 0)
                    createClone.Add(col.Name, col.DbType, col.Size, col.ColumnFlags);
                else
                    createClone.Add(col.Name, col.DbType, col.ColumnFlags);

                //if we are not an auto assign then add the column to the select and the insert
                if (col.AutoAssign == false)
                {
                    sel.Field(col.Name);
                    ins.Field(col.Name);
                }
            }
            sel = sel.From(tbl.Schema, tbl.Name);
            //add any filtering to the select statement

            //Append the select as part of the insert
            ins.Select(sel);

            //statement construction complete

            //drop any existing cloned table
            db.ExecuteNonQuery(DBQuery.Drop.Table(cloneTbl).IfExists());
            //IF EXISTS (SELECT *
		    //          FROM [INFORMATION_SCHEMA].[TABLES]
		    //          WHERE  ([TABLE_NAME] = 'DQSL_COURSES_Clone') ) 
	        //              DROP TABLE [DQSL_COURSES_Clone]

            db.ExecuteNonQuery(createClone);
            //CREATE TABLE [dbo].[DQSL_COURSES_Clone] (
		    //              [CourseID] CHAR(4) PRIMARY KEY NOT NULL, 
		    //              [Title] NVARCHAR(100) NOT NULL, 
		    //              [Credits] FLOAT(4) NOT NULL, 
		    //              [DepartmentID] INT NOT NULL, 
		    //              [Description] NVARCHAR(1000) NULL)

            //execute the insert into select from
            db.ExecuteNonQuery(ins);
            //INSERT INTO [DQSL_COURSES_Clone]
	        //          ([CourseID], [Title], [Credits], [DepartmentID], [Description])
	        //              (SELECT [CourseID], [Title], [Credits], [DepartmentID], [Description]
		    //               FROM [dbo].[DQSL_COURSES]) 

        }


        [TestMethod()]
        public void SQLClient_22_UpdateExpressions()
        {

            DBQuery upd = DBQuery.Update("MyTable").Set("Salary", DBField.Field("Salary") + DBConst.Int32(4000));
            string all = upd.ToSQLString(this.Database);
            TestContext.WriteLine("Update Statement : " + all);
        }


        [TestMethod()]
        public void SQLClient_23_RoundFunction()
        {
            DBParam amount = DBParam.ParamWithValue("amount",10);
            DBQuery sel = DBQuery.SelectAll()
                                 .Where(DBFunction.Function("Round", amount, DBConst.Int32(2)), Compare.GreaterThanEqual, DBField.Field("MinAmount"))
                                 .AndWhere(DBFunction.Function("Round", amount, DBConst.Int32(2)), Compare.LessThanEqual, DBField.Field("MaxAmount"));

            
            string all = sel.ToSQLString(this.Database);
            TestContext.WriteLine("Update Statement : " + all);
        }

        #region public void SQLClient_100_TearDownSchema()

        /// <summary>
        /// Clean up all the created items. If you create anything then the corresponding drop should be in here too.
        /// </summary>
        [TestMethod()]
        public void SQLClient_100_TearDownSchema()
        {
            TestContext.WriteLine("Cleaning up database objects");

            DBQuery[] all = new DBQuery[] {

                DBQuery.Drop.StoredProcedure(InsertOnSiteCourseSproc).IfExists(),
                DBQuery.Drop.StoredProcedure(InsertOnlineCourseSproc).IfExists(),

                DBQuery.Drop.View(Schools.Schema,OnlyOnSiteCoursesView).IfExists(),
                DBQuery.Drop.View(Schools.Schema,OnlyOnlineCoursesView).IfExists(),

                //DBQuery.Drop.Index(Schools.Schema,"idx_DeptName").IfExists(),     //uses the INFORMATION_SCHEMA.INDEXES view
                //DBQuery.Drop.Index(Schools.Schema,"idx_CourseTitle").IfExists(),  //if your version supports then execute the drop.
                                                                                    //Should be dropped with the tables anyway

                DBQuery.Drop.Table(Schools.Schema,Schools.Instructor.Table).IfExists(),
                DBQuery.Drop.Table(Schools.Schema,Schools.Person.Table).IfExists(),
                DBQuery.Drop.Table(Schools.Schema,Schools.OnlineCourse.Table).IfExists(),
                DBQuery.Drop.Table(Schools.Schema,Schools.OnsiteCourse.Table).IfExists(),
                DBQuery.Drop.Table(Schools.Schema,Schools.Courses.Table).IfExists(),
                DBQuery.Drop.Table(Schools.Schema,Schools.Departments.Table).IfExists()
            };

            DBDatabase db = this.Database;
            Exception failure = null;
            int failcount = 0;

            foreach (DBQuery q in all)
            {
                try
                {
                    db.ExecuteNonQuery(q);
                }
                catch (Exception ex)
                {
                    if (null == failure)
                        failure = ex;
                    failcount++;
                    TestContext.WriteLine("Clean up failed for '{0}'\r\n{1}", q.ToSQLString(db), ex);
                }
            }

            if (failcount > 0)
            {
                throw new Exception("There were '" + failcount.ToString() + 
                                    "' failures executing the drop schema tests. The first of which was '" 
                                    + failure.Message + "'", failure);
            }
        }

        #endregion

    }
}
