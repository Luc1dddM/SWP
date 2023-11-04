using SWP_CarService_Final.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SWP_CarService_Final.Services
{
    public class CategoryServices : DBContext
    {
        public List<Category> getAllCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT *FROM [SWP].[dbo].[Category] ORDER BY CAST(SUBSTRING(category_id, PATINDEX('%[0-9]%', category_id), LEN(category_id)) AS INT) desc", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                        categories.Add(category);
                    }
                }
            }
            finally { connection.Close(); }
            return categories;
        }

        public List<Category> getComponentCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SWP].[dbo].[Category] where category_type = 'component'", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                        categories.Add(category);
                    }
                }
            }
            finally { connection.Close(); }
            return categories;
        }

        public List<Category> getBrandCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SWP].[dbo].[Category] where category_type = 'Brand'", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                        categories.Add(category);
                    }
                }
            }
            finally { connection.Close(); }
            return categories;
        }

        public Category GetCategoryByID(string id) 
        {
            Category category = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT * FROM [SWP].[dbo].[Category] WHERE category_id = @category_id";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("category_id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return category;
        }

        public int getNumberOfCategory()
        {
            Category category = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT top 1 * FROM Category ORDER BY CAST(SUBSTRING(category_id, PATINDEX('%[0-9]%', category_id), LEN(category_id)) AS INT) desc";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return category == null ? 0:int.Parse(category.category_id.Substring(3));
        }

        public void createCategory(Category nCategory)
        {
            try
            {
                string id = "CTG" + (getNumberOfCategory() + 1);
                connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Category] ([category_id],[category_name],[category_type],[active]) " +
                                                        "values(@category_id,@category_name, @category_type, @active)", connection);
                    command.Parameters.AddWithValue("category_id", id);
                    command.Parameters.AddWithValue("category_name", nCategory.category_name);
                    command.Parameters.AddWithValue("category_type", nCategory.category_type);
                    command.Parameters.AddWithValue("active", nCategory.active);
                    command.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void editCategory(Category ncategory)
        {
            Category category = new Category();
            
            try
            {
                category = GetCategoryByID(ncategory.category_id);
                connection.Open();
                if (category != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [SWP].[dbo].[Category] SET[category_name] = @category_name, " +
                                                        "[category_type] = @category_type, [active] = @active " +
                                                        "WHERE [category_id] = @category_id", connection);
                    command.Parameters.AddWithValue("category_name", ncategory.category_name);
                    command.Parameters.AddWithValue("category_type", ncategory.category_type);
                    command.Parameters.AddWithValue("active", ncategory.active);
                    command.Parameters.AddWithValue("category_id", ncategory.category_id);
                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("This category does not already exist.");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public List<Category> getCategoriesNotAdd(List<PartCategory> partCategories)
        {
            List<Category> categories = new List<Category>();
            string except = null;
            try
            {
                if(partCategories != null)
                {
                    foreach (PartCategory partCategory in partCategories)
                    {
                        except += " and category_id != '" + partCategory.category_id +"' ";
                    }
                }


                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SWP].[dbo].[Category] where 1=1 "+except, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category()
                        {
                            category_id = reader.GetString(0),
                            category_name = reader.GetString(1),
                            category_type = reader.GetString(2),
                            active = reader.GetBoolean(3)
                        };
                        categories.Add(category);
                    }
                }
            }
            finally { connection.Close(); }
            return categories;
        }


    }
}
