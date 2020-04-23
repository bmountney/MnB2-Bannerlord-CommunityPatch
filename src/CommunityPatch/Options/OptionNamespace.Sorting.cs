using System;
using System.Collections.Generic;

namespace CommunityPatch.Options {

  public partial class OptionNamespace {

    public static bool operator <(OptionNamespace left, OptionNamespace right)
      => Comparer<OptionNamespace>.Default.Compare(left, right) < 0;

    public static bool operator >(OptionNamespace left, OptionNamespace right)
      => Comparer<OptionNamespace>.Default.Compare(left, right) > 0;

    public static bool operator <=(OptionNamespace left, OptionNamespace right)
      => Comparer<OptionNamespace>.Default.Compare(left, right) <= 0;

    public static bool operator >=(OptionNamespace left, OptionNamespace right)
      => Comparer<OptionNamespace>.Default.Compare(left, right) >= 0;

    public int CompareTo(OptionNamespace other) {
      if (ReferenceEquals(this, other))
        return 0;
      if (ReferenceEquals(null, other))
        return 1;

      var optionsComparison = _store.CompareTo(other._store);
      if (optionsComparison != 0)
        return optionsComparison;

      return string.Compare(Namespace, other.Namespace, StringComparison.Ordinal);
    }

  }

}