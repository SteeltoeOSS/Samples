class Version:

    def __init__(self, version):
        self.version = version

    def __repr__(self):
        return self.version

    def __eq__(self, other):
        return Comparator().compare(self.version, other.version) == 0

    def __ne__(self, other):
        return not self == other

    def __lt__(self, other):
        return Comparator().compare(self.version, other.version) < 0

    def __le__(self, other):
        return Comparator().compare(self.version, other.version) <= 0

    def __gt__(self, other):
        return Comparator().compare(self.version, other.version) > 0

    def __ge__(self, other):
        return Comparator().compare(self.version, other.version) >= 0


class Comparator:

    '''
    A comparator that semantically sorts versions. This comparator considers,
    e.g., "10" to be greater than "9", in contrast to a lexical comparison.

    Return:
         0 if version a == version b
        <0 if version a < version b
        >0 if version a > version b

    Rules:
        * version strings are broken into nodes delimited by '.'
        * comparison is initially on the first node of each version
            * if each node can both be converted to an integer, they are compared as such
            * otherwise, the nodes are compared as strings
        * if the comparison result of the first nodes is not 0, that result is returned
        * else, the first nodes are equal
            * if neither version has addition nodes, 0 is returned
            * if each version has additional nodes, those nodes are compared as above
            * else the version with the no more nodes remaining is considered the lesser version
    '''
    def compare(self, a, b):
        if a is None or b is None:
            raise ValueError('version string cannot be None')
        if not(isinstance(a, Version) or isinstance(a, str)) or not(isinstance(b, Version) or isinstance(b, str)):
            raise TypeError('version must be an instance of a Version or str')
        if not a or not b:
            raise ValueError('version string cannot be empty')
        def compareNodes(anodes, bnodes):
            if len(anodes) == 0 and len(bnodes) == 0:
                return 0
            if len(anodes) == 0:
                return -1
            if len(bnodes) == 0:
                return 1
            anode, bnode = anodes[0], bnodes[0]
            try:
                anode, bnode = int(anode), int(bnode)
            except ValueError:
                pass
            if anode > bnode:
                return 1
            elif anode < bnode:
                return -1
            else:
                return compareNodes(anodes[1:], bnodes[1:])
        return compareNodes(a.split('.'), b.split('.'))
