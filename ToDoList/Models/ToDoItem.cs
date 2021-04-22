namespace ToDoList.Models
{
    public class ToDoItem
    {
        // code first 
        public int Id { get; set; } // id is primary key
        public string Text { get; set; } // textul elementului din lista
        public bool IsDone { set; get; }
        public virtual ApplicationUser User { set; get; } // link to a user
    }
}