namespace ToDo.Domain
{
    public interface IToDoElement
    {
        // code first 
        public int Id { get; set; } // id is primary key
        public string Text { get; set; } // textul elementului din lista
        public bool IsDone { set; get; }
        public ApplicationUser User { set; get; }
    }
}