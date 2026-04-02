using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Domain.Entities;


    public class Todo
    {

      public Todo(string title, string? description)
      {
         if (string.IsNullOrWhiteSpace(title))
         { 
           throw new ArgumentException("Title cannot be empty.", nameof(title));
         }

         Title = title;
         Description = description;
         IsCompleted = false;
         CreatedAt = DateTime.UtcNow;
      }      


        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        
        protected Todo() { }


        public void Complete()
        {
          IsCompleted = true;
          CompletedAt = DateTime.UtcNow;
        }

        public void Update(string title, string? description)
        {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");  

          Title = title;
          Description = description;
          Title = title;
          Description = description;
        }   





}

