using Alachisoft.NCache.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCacheAppFabricConsoleUI
{
    internal static class TagsTests
    {
        internal static void AddItemWithTagsInRegion()
        {
			try
			{
				Logger.PrintTestStartInformation("Adding non existing item with tags in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item added successfuly with tag array");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added successfuly with tag array");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
        }

		internal static void AddItemWithNullTagArrayInRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non existing item with null tag array in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = null;

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item added successfuly with null tag array");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added successfuly with null tag array");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void AddItemWithTagArrayHavingNullElementsInRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non existing item with tag array having null elements in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					null
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item added successfuly with tag array having null elements");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added successfuly with tag array having null elements");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void AddItemWithZeroLengthTagArrayInRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non existing item with tag array having 0 elements in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				var tags = new List<DataCacheTag>();

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item added successfuly with tag array having 0 elements");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added successfuly with tag array having 0 elements");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting items from existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsInRegion(myRegion);

				if (objects.Count() == 1)
				{
					Logger.PrintSuccessfulOutcome("Item retrieved with GetObjectsInRegion call");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not retrieved with GetObjectsInRegion call");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting items from non existing region");

				string myRegion = Guid.NewGuid().ToString();

				var objects = Program.myDefaultCache.GetObjectsInRegion(myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("GetObjectsInRegion call returned null");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("GetObjectsInRegion call returned 0 elements");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with a given tag in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByTag(new DataCacheTag("tag1"), myRegion);

				if (objects != null && objects.Count() == 1)
				{
					Logger.PrintSuccessfulOutcome("Item gotten successfuly with tag");
				}
				else
				{
					Logger.PrintFailureOutcome("Item retrieval with tag failed");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByTagInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with a given tag in non existing region");

				string myRegion = Guid.NewGuid().ToString();


				var objects = Program.myDefaultCache.GetObjectsByTag(new DataCacheTag("tag1"), myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByNonExistingTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with a non existing tag in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByTag(new DataCacheTag("tag4"), myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten with non existant tag as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with non existing tag as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByNullTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with null tag in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByTag(null, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten with non existant tag as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with non existing tag as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAnyTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with any given tags in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAnyTag(new[] { new DataCacheTag("tag1") }, myRegion);

				if (objects != null && objects.Count() == 1)
				{
					Logger.PrintSuccessfulOutcome("Item gotten successfuly with given tag");
				}
				else
				{
					Logger.PrintFailureOutcome("Item retrieval with tag failed");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAnyNonExistingTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with any given non existing tags in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAnyTag(new[] { new DataCacheTag("tag3") }, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAnyNullTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with any given null tags in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAnyTag(new DataCacheTag[] { null }, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAnyNullTagArrayInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with any tag array which is null in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAnyTag(null, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAnyZeroTagArrayInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with any zero length tag list in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, tags, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAnyTag(new List<DataCacheTag>(), myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}

		internal static void GetItemsByAllTagInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing items with all given tags in existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				DataCacheTag[] tags = new DataCacheTag[]
				{
					new DataCacheTag("tag1"),
					new DataCacheTag("tag2")
				};

				Program.myDefaultCache.CreateRegion(myRegion);
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, new[]{ new DataCacheTag("tag1") }, myRegion);

				var objects = Program.myDefaultCache.GetObjectsByAllTags(tags, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null Item enumerable gotten as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("0 Item gotten successfuly with as expected");
				}
			}
			catch (Exception e)
			{
				Logger.PrintDataCacheException(e);
			}
			finally
			{
				Logger.PrintBreakLine();
			}
		}
	}
}
