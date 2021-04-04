using Microsoft.EntityFrameworkCore.Migrations;

namespace Course_Project.Migrations
{
    public partial class AddFullText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("create fulltext catalog PostFull", true);
            migrationBuilder.Sql("create fulltext index on dbCourse.Posts(Title TYPECOLUMN varchar LANGUAGE 1033,Description TYPECOLUMN varchar LANGUAGE 1033) KEY INDEX IX_Posts_Id ON PostFull", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
