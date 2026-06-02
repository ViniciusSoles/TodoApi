using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Domain.Shared;

    public class PaginationParams
    {

    private readonly int _maxPageSize;
    private int _pageSize;

    public PaginationParams(int defaultPageSize = 10, int maxPageSize = 50)
    {
        _maxPageSize = maxPageSize;
        _pageSize = defaultPageSize;
    }

    public int Page { get; set; } = 1;

       public int PageSize
       {
           get => _pageSize;
           set => _pageSize = value > _maxPageSize ? _maxPageSize : value;   
    
       }   

    }

