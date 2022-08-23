using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Collections.Specialized;

namespace GlobalControls.Paging
{
	/// <summary>
	/// Summary description for Paging.
	/// </summary>
	/// 

	public class PagingManager
	{
		private DataView _RecordSet;
		private int _RecordsPerPage;
		private PagingStyle _PagingStyle;
		private int _Page;
		private int _LPage;
		public enum PagingStyle{Numbers=1,Text=2,ResultPageSet=3}
		private string _PagingTitle;
		private string _PagingLinks;
		private string _ResultPageHeader = "Page 1 of 1";
		private string _ResultPageFooter = "Page 1 of 1";

		private ArrayList _PagesArray = new ArrayList();

		//pages array contains an array of all pages - 
		//ie an instance of the PagesData struct with all properties for each page
		public ArrayList PagesArray
		{
			set{_PagesArray.Add((object)value);}
			get{return _PagesArray;}
		}

		//the following properties return page layout links to handle various paging styles

		public string PagingTitle
		{
			set{_PagingTitle=value;}
			get{return _PagingTitle;}
		}

		public string PagingLinks
		{
			set{_PagingLinks=value;}
			get{return _PagingLinks;}
		}

		//show next number or records per page records
		//1-10, 1-15...
		public string ResultPageHeader
		{
			get{return _ResultPageHeader;}
		}
		
		//show pages link as collection with all pages 
		//scrolled shown and current page non-linkable
		public string ResultPageFooter
		{
			get{return _ResultPageFooter;}
		}

		//holds individual page properties
		//pagenumber = each page of data
		//startindex = first record for page
		//endindex = last record for page
		public class PageData
		{
			private int _PageNumber;
			private int _StartIndex;
			private int _EndIndex;

			public int PageNumber
			{
				set{_PageNumber = value;}
				get{return _PageNumber;}
			}

			public int StartIndex
			{
				set{_StartIndex = value;}
				get{return _StartIndex;}
			}

			public int EndIndex
			{
				set{_EndIndex = value;}
				get{return _EndIndex;}
			}
		}

		//Paging Manager constructor - handles setting of initial paging values
		//all but the DataView object are set as default - these values do not have
		//to be set for paging to work
		public PagingManager()
		{
			_RecordSet = null;
			_RecordsPerPage = 25;
			_PagingStyle = PagingStyle.Numbers;
			_Page = 1;
			_LPage = 1;
			_PagingTitle = "";
			_PagingLinks = "";
		}

		//GetPage is the primary and only public function of the paging manager
		//every pages postback event should implement this function to set
		//the correct page data, title, and page links
		//1st GetPage Overload - uses all default values
		public DataView GetPage(DataView RecordSet) 
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;

			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		//2nd GetPage Overload
		public DataView GetPage(DataView RecordSet, int Page) 
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_Page = Page;

			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		//3rd GetPage Overload
		public DataView GetPage(DataView RecordSet, int Page, int RecordsPerPage) 
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_Page = Page;
			_RecordsPerPage = RecordsPerPage;

			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		public DataView GetPage(DataView RecordSet, int Page, int RecordsPerPage, PagingStyle Style) //4th GetPage Overload
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_Page = Page;
			_RecordsPerPage = RecordsPerPage;
			_PagingStyle = Style;

			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		public DataView GetPage(DataView RecordSet, int RecordsPerPage, PagingStyle Style) //4th GetPage Overload
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_RecordsPerPage = RecordsPerPage;
			_PagingStyle = Style;
			
			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		public DataView GetPage(DataView RecordSet, PagingStyle Style) //4th GetPage Overload
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_PagingStyle = Style;
			
			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		public DataView GetPage(DataView RecordSet, int Page, int LPage, int RecordsPerPage, PagingStyle Style) //4th GetPage Overload
		{
			if(RecordSet.Count==0)
				throw new System.Exception("Record Set Is Empty");
			else
				_RecordSet = RecordSet;
			
			_Page = Page;
			_LPage = LPage;
			_RecordsPerPage = RecordsPerPage;
			_PagingStyle = Style;

			if(_RecordSet.Count<=_RecordsPerPage)
			{
				_PagingLinks = "Page 1 of 1";
				return _RecordSet;
			}
			else
			{
				SetPagesArray();
				RenderPaging();
				return GetDataSubset();
			}
		}

		//does calcs to determine how many pages need to be created and
		//sets the pagenumber, start, and end index for each
		private void SetPagesArray()
		{
			int mNumberPages = 0,mModPages=0,mMaxEvenRecords=0,mCount=0;
			mNumberPages = Convert.ToInt32(Math.Round(Convert.ToDouble(_RecordSet.Count/_RecordsPerPage)));
			mMaxEvenRecords = mNumberPages * _RecordsPerPage;
			mModPages =_RecordSet.Count%_RecordsPerPage;
			for(int i=1;i<=mNumberPages;i++)
			{
				PageData newPage = new PageData();
				newPage.PageNumber = i;
				newPage.StartIndex = mCount;
				newPage.EndIndex = ((mCount + _RecordsPerPage) - 1);
				this.PagesArray.Add(newPage);
				mCount += _RecordsPerPage;
			}
			if(mModPages>0)
			{
				mNumberPages += 1;
				PageData newPage = new PageData();
				newPage.PageNumber = mNumberPages;
				newPage.StartIndex = mMaxEvenRecords;
				newPage.EndIndex = _RecordSet.Count - 1;
				this.PagesArray.Add(newPage);
			}
		}

		//set the html for the PagingTitle and PagingLinks
		private void RenderPaging()
		{
			switch(_PagingStyle)
			{
				case PagingStyle.Numbers:
					PagedByNumbers();
					break;
				case PagingStyle.Text:
					PagedByText();
					break;
				case PagingStyle.ResultPageSet:
					ResultPageSet();
					break;
			}
		}

		private void PagedByNumbers()
		{
			string ScriptName = HttpContext.Current.Request.ServerVariables["Script_Name"].ToString();
			this.PagingLinks = "Page ";
			foreach(PageData page in _PagesArray)
			{
				this.PagingLinks += "<a href=" + ScriptName + "?page=" + page.PageNumber + GetQueryString(HttpContext.Current.Request.QueryString) + ">" + page.PageNumber + "</a> ";
			}
		}

		private void PagedByText()
		{
			string ScriptName = HttpContext.Current.Request.ServerVariables["Script_Name"].ToString();
			string Prev="",Next="",Pages="";
			PageData pageData = (PageData)PagesArray[this._PagesArray.Count-1];
			Pages = "Page " + this._Page + " of " + pageData.PageNumber;

			if(this._Page>1)
				Prev = "<a href=" + ScriptName + "?page=" + (this._Page-1) + GetQueryString(HttpContext.Current.Request.QueryString) + ">Prev</a>";

			if(this._Page<pageData.PageNumber)
				Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next</a>";

			this.PagingLinks = Prev + " " + Pages + " " + Next;
		}

		private void ResultPageSet()
		{
			SetResultPageHeader();
			SetResultPageFooter();
		}

		private void SetResultPageHeader()
		{
//			if(HttpContext.Current.Request["lpage"]!=null)
//				_LPage = Int32.Parse(HttpContext.Current.Request["lpage"].ToString());
//			else
//			{
//				if(this._PagesArray.Count<=2)
//					_LPage = this._PagesArray.Count;
//				else
//					_LPage = 2;
//			}

			if(this._PagesArray.Count<=_LPage)
					_LPage = this._PagesArray.Count;
			
			string ScriptName = HttpContext.Current.Request.ServerVariables["Script_Name"].ToString();
			string Prev="",Next="",Pages="";
			PageData LastPage = (PageData)PagesArray[this._PagesArray.Count-1];
			PageData CurrentPage = (PageData)PagesArray[this._Page-1];
			Pages = Convert.ToString(CurrentPage.StartIndex + 1) + "-" + Convert.ToString(CurrentPage.EndIndex + 1) + " of " + this._RecordSet.Count;

			if(this._Page>1)
				Prev = "<a href=" + ScriptName + "?page=" + (this._Page-1) + "&lpage=" + _LPage.ToString() + GetQueryString(HttpContext.Current.Request.QueryString) + ">&#060;&#060;Previous</a>";

			if(this._Page<LastPage.PageNumber)
			{
				if(_LPage<LastPage.PageNumber)
					Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + "&lpage=" + Convert.ToString(_LPage+1) + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next&#062;&#062;</a>";
				else
					Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + "&lpage=" + _LPage.ToString() + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next&#062;&#062;</a>";
			}

			_ResultPageHeader = "Result Page " + Prev + " " + Pages + " " + Next;
		}

		private void SetResultPageFooter()
		{
			string ScriptName = HttpContext.Current.Request.ServerVariables["Script_Name"].ToString();
			string Prev="",Next="",Pages=" ";
			PageData pageData = (PageData)PagesArray[this._PagesArray.Count-1];

//			int _LPage = 0;
//			if(HttpContext.Current.Request["lpage"]!=null)
//				_LPage = Int32.Parse(HttpContext.Current.Request["lpage"].ToString());
//			else
//			{
//				if(this._PagesArray.Count<=2)
//					_LPage = this._PagesArray.Count;
//				else
//					_LPage = 2;
//			}

			if(this._PagesArray.Count<=_LPage)
				_LPage = this._PagesArray.Count;

			for(int i=1;i<=_LPage;i++)
			{
				if(i==this._Page)
					Pages += i + " ";
				else
					Pages += "<a href=" + ScriptName + "?page=" + i + "&lpage=" + _LPage.ToString() + GetQueryString(HttpContext.Current.Request.QueryString) + ">" + i + "</a> ";
			}

			if(this._Page>1)
				Prev = "<a href=" + ScriptName + "?page=" + (this._Page-1) + "&lpage=" + _LPage + GetQueryString(HttpContext.Current.Request.QueryString) + ">&#060;&#060;Previous</a>";

//			if(this._Page<pageData.PageNumber&&_LPage<pageData.PageNumber)
//				Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + "&lpage=" + Convert.ToString(_LPage+1) + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next&#062;&#062;</a>";

			if(this._Page<pageData.PageNumber)
			{
				if(_LPage<pageData.PageNumber)
					Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + "&lpage=" + Convert.ToString(_LPage+1) + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next&#062;&#062;</a>";
				else
					Next = "<a href=" + ScriptName + "?page=" + (this._Page+1) + "&lpage=" + _LPage.ToString() + GetQueryString(HttpContext.Current.Request.QueryString) + ">Next&#062;&#062;</a>";
			}

			_ResultPageFooter = "Result Page " + Prev + " " + Pages + " " + Next;
		}

		//using all preset member variables to determine the dataset to return
		//for each page
		private DataView GetDataSubset()
		{
			//create temp table to transfer selected rows from original datastore object
			DataTable DataTableSubset = new DataTable();

			//set temp tables datacolumncollection for data binding
			foreach(DataColumn Column in _RecordSet.Table.Columns)
			{
				DataTableSubset.Columns.Add(Column.ColumnName);
			}
			PageData pageData = (PageData)PagesArray[this._Page-1];

			//import specific rows to temp table
			for(int i=pageData.StartIndex;i<=pageData.EndIndex;i++)
			{
				DataTableSubset.ImportRow(_RecordSet.Table.Rows[i]);
			}
			DataView DataSubset = new DataView(DataTableSubset);
			return DataSubset;
		}

		public string GetQueryString(NameValueCollection RequestQueryString)
		{
			string Key = "";
			string Value = "";
			string queryString = "";

			for(int i=0;i<RequestQueryString.Keys.Count;i++) 
			{
				if (RequestQueryString.Keys.Get(i) != null)
				{
					Key = RequestQueryString.Keys.Get(i).ToString();
					Value = RequestQueryString.GetValues(i).GetValue(0).ToString();
					if(Key.ToString().ToLower()!="page"&&Key.ToString().ToLower()!="lpage")
					{
						queryString += "&" + Key + "=" + Value;
					}
				}
			}
			return queryString;
		}
	}
}

