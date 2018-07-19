import sure
import unittest
import version

class VersionTest(unittest.TestCase):

    def testStringRepresentation(self):
        myversion = version.Version('1.2.3')
        repr(myversion).should.equal('1.2.3')

    def testEqual(self):
        a = version.Version('1.2.3')
        b = version.Version('1.2.3')
        a.should.equal(b)

    def testNotEqual(self):
        a = version.Version('1.2.3')
        b = version.Version('1.2.4')
        a.should_not.equal(b)

    def testGreaterThan(self):
        a = version.Version('10')
        b = version.Version('9')
        (a > b).should.be.true

    def testGreaterThanOrEquals(self):
        a = version.Version('10')
        b = version.Version('9')
        c = version.Version('10')
        (a >= b).should.be.true
        (a >= c).should.be.true

    def testLessThan(self):
        a = version.Version('9')
        b = version.Version('10')
        (a < b).should.be.true

    def testLessThanOrEqual(self):
        a = version.Version('9')
        b = version.Version('10')
        c = version.Version('9')
        (a <= b).should.be.true
        (a <= c).should.be.true
        
class ComparatorTest(unittest.TestCase):

    def setUp(self):
        self.comparator = version.Comparator()

    def testEqual(self):
        (self.comparator.compare('1.2.3', '1.2.3') == 0).should.be.true

    def testNumericComparison(self):
        (self.comparator.compare('10', '9') > 0).should.be.true
        (self.comparator.compare('9', '10') < 0).should.be.true

    def testAlphaComparsion(self):
        (self.comparator.compare('z', 'a') > 0).should.be.true
        (self.comparator.compare('a', 'z') < 0).should.be.true

    def testAlphaNumberComparison(self):
        (self.comparator.compare('a', '1') > 0).should.be.true
        (self.comparator.compare('1', 'a') < 0).should.be.true

    def testDifferentNodeLengths(self):
        (self.comparator.compare('1.2.3', '1.2') > 0).should.be.true
        (self.comparator.compare('1.2', '1.2.3') < 0).should.be.true

    def testValueError(self):
        self.assertRaises(ValueError, self.comparator.compare, '1.2.3', '')
        self.assertRaises(ValueError, self.comparator.compare, '', '1.2.3')
        self.assertRaises(ValueError, self.comparator.compare, '1.2.3', None)
        self.assertRaises(ValueError, self.comparator.compare, None, '1.2.3')

    def testTypeError(self):
        self.assertRaises(TypeError, self.comparator.compare, '1.2.3', 0)
        self.assertRaises(TypeError, self.comparator.compare, 0, '1.2.3')

if __name__ == '__main__':
    unittest.main()
