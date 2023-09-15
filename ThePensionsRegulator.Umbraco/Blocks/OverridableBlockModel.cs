using System.Collections;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public abstract class OverridableBlockModel<T> : IEnumerable<T> where T : IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        protected readonly IList<T> Items = new List<T>();
        protected static Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> DefaultFilter = x => true;
        protected Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> BaseFilter { get; set; } = DefaultFilter;

        protected void ConvertBlockModelPropertyToOverridable<TBaseModel, TOverridableModel>(
            string propertyEditorAlias,
            IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> overridableItem,
            IPublishedProperty property,
            Func<TBaseModel, TOverridableModel> baseToOverridableFactory,
            Func<TOverridableModel> nullToOverridableFactory)
        {
            if (property.PropertyType.EditorAlias == propertyEditorAlias)
            {
                var overriddenNestedBlockModel = overridableItem.Content.Value<TOverridableModel>(property.Alias);
                if (overriddenNestedBlockModel == null)
                {
                    var nestedBlockModel = overridableItem.Content.Value<TBaseModel>(property.Alias);
                    if (nestedBlockModel != null)
                    {
                        overriddenNestedBlockModel = baseToOverridableFactory(nestedBlockModel);
                    }
                    else
                    {
                        overriddenNestedBlockModel = nullToOverridableFactory();
                    }
                }
                overridableItem.Content.OverrideValue(property.Alias, overriddenNestedBlockModel!);
            }
        }

        protected void CopyFilterToDescendantBlockLists(IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter)
        {
            foreach (var block in blocks)
            {
                var models = block.Content.FindOverridableBlockModels().OfType<OverridableBlockListModel>();
                foreach (var model in models)
                {
                    model.Filter = filter;
                    CopyFilterToDescendantBlockLists(model, filter);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the unfiltered list of blocks.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the unfiltered list of blocks.
        /// </summary>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)Items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the unfiltered list of blocks.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>Gets the <see cref="T" /> with the specified content key.</summary>
        /// <value>The <see cref="T" />.</value>
        /// <param name="contentKey">The content key.</param>
        /// <returns>The <see cref="T" /> with the specified content key.</returns>
        public T? this[Guid contentKey] => Items.FirstOrDefault(x => x.Content.Key == contentKey);

        /// <summary>Gets the <see cref="T" /> with the specified content UDI.</summary>
        /// <value>The <see cref="T" />.</value>
        /// <param name="contentUdi">The content UDI.</param>
        /// <returns>The <see cref="T" /> with the specified content UDI.</returns>
        public T? this[Udi contentUdi] => contentUdi is GuidUdi guidUdi
            ? Items.FirstOrDefault(x => x.Content.Key == guidUdi.Guid)
            : default;

        /// <summary>
        /// Gets or sets a block from the unfiltered list of blocks
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        /// <summary>
        /// Searches for the specified <see cref="T" /> and returns the zero-based index of the first occurrence within the entire <see cref="IList{T}">.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="IList{T}">. The value can be null.</param>
        /// <returns>The zero-based index of the first occurrence of the <see cref="T" /> within the entire 
        /// <see cref="IList{T}">, if found; otherwise, -1.</returns>
        public int IndexOf(T value) => Items.IndexOf(value);

        /// <summary>
        /// Gets the number of <see cref="T" /> instances contained in the <see cref="IList{T}">. 
        /// </summary>
        /// <returns>The number of <see cref="T" /> instances contained in the <see cref="IList{T}">.</returns>
        public int Count { get => Items.Count; }

        /// <summary>
        /// Determines whether an <see cref="T" /> is in the <see cref="IList{T}">.
        /// </summary>
        /// <param name="value">The <see cref="T" /> to locate in the <see cref="IList{T}">. The value can be null.</param>
        /// <returns><c>true</c> if value is found in the <see cref="IList{T}">; otherwise, <c>false</c>.</returns>
        public bool Contains(T value) => Items.Contains(value);

        /// <summary>
        /// Copies the entire <see cref="IList{T}"> to a compatible one-dimensional <see cref="Array" />, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the <see cref="T" /> 
        /// instances copied from <see cref="IList{T}">. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>     
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>     
        /// <exception cref="ArgumentException">The number of <see cref="T" /> instances in the source <see cref="IList{T}"> 
        /// is greater than the available space from index to the end of the destination array.</exception>     
        public void CopyTo(T[] array, int index) => Items.CopyTo(array, index);
    }
}