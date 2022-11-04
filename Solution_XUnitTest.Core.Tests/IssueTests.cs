using Solution_XUnitTest.Core.Test2.Exceptions;
using Solution_XUnitTest.Core.Test2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solution_XUnitTest.Core.Tests
{
        public class IssueTests
        {
            #region Ctor
            [Fact]
            public void Ctor_DescriptionIsNull_ReturnsInvalidIssueDescriptionException()
            {
                //arrange
                //act
                //assert
                Assert.Throws<InvalidIssueDescriptionException>(() => new Issue(null, Priority.High, Category.Software));
            }

            [Fact]
            public void Ctor_DescriptionIsWhiteSpace_ReturnsInvalidIssueDescriptionException()
            {
                //arrange
                //act
                //assert
                Assert.Throws<InvalidIssueDescriptionException>(() => new Issue("  ", Priority.High, Category.Software));
            }

            [Fact]
            public void Ctor_CreatedDateIsNull_ReturnsDateNow()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.High, Category.Software);
                //act
                //le datetime est une struct s'il n'est pas nullable impossible qu'il prend un null
                //donc la valeur par defaut 0001-01-01
                var act = sut.CreatedAt;
                var expected = default(DateTime);
                //assert
                Assert.False(act == expected);

            }
            #endregion

            #region GenerateKey
            //comme GenerateKey est privé => Reflection pour acceder a notre methode
            [Fact]
            public void GenerateKey_WithIssueValidProperties_ReturnIssueKey()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var actual = methodInfo.Invoke(sut, null).ToString();
                var expected = "HW-2022-L-ABCD1234";
                //assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Length, actual.Length);
            }

            [Fact]
            public void GenerateKey_WithIssueHardware_ReturnIssueKeyFirstSegmentHW()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.High, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var actual = methodInfo.Invoke(sut, null).ToString();
                var expected = "HW";
                //assert
                Assert.NotNull(actual);
                Assert.Equal(expected, actual.Split('-')[0]);
            }


            [Fact]
            public void GenerateKey_WithIssuePriorityLow_ReturnIssueKeyThirdSegmentL()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var actual = methodInfo.Invoke(sut, null).ToString();
                var expected = "L";
                //assert
                Assert.NotNull(actual);
                Assert.Equal(expected, actual.Split('-')[2]);
            }

            [Fact]
            public void GenerateKey_WithIssueYear_ReturnIssueKeySecondSegmentYYYY()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var actual = methodInfo.Invoke(sut, null).ToString();
                var expected = "2022";
                //assert
                Assert.NotNull(actual);
                Assert.Equal(expected, actual.Split('-')[1]);
            }



            [Fact]
            public void GenerateKey_WithIssueValidProperties_ReturnIssueKeyFourthSegment8Alphanumeric()
            {
                //arrange
                var sut = new Issue("Issue #1", Priority.Low, Category.Hardware, new DateTime(2022, 10, 11, 12, 30, 00));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var fourthSegment = methodInfo.Invoke(sut, null).ToString().Split('-')[3];
                var isAplphanumeric = fourthSegment.All(x => char.IsLetterOrDigit(x));
                //assert
                Assert.True(isAplphanumeric);

            }

            #endregion

            #region GenerateKey Resume with Theory
            //with Fact without params
            //with Theory avec un jeux de données (comme multiple Facts en un seul)

            [Theory]
            [InlineData("Issue #1", Priority.Urgent, Category.Software, "2000-10-10", "SW-2000-U-ABCD1234")]
            [InlineData("Issue #1", Priority.Low, Category.Software, "2022-10-10", "SW-2022-L-ABCD1234")]
            [InlineData("Issue #1", Priority.Low, Category.UnKnown, "2018-10-10", "NA-2018-L-ABCD1234")]
            [InlineData("Issue #1", Priority.Low, Category.Hardware, "1992-10-10", "HW-1992-L-ABCD1234")]
            [InlineData("Issue #1", Priority.Medium, Category.Hardware, "2003-10-10", "HW-2003-M-ABCD1234")]
            [InlineData("Issue #1", Priority.High, Category.Hardware, "2015-10-10", "HW-2015-H-ABCD1234")]
            [InlineData("Issue #1", Priority.Urgent, Category.Hardware, "1980-10-10", "HW-1980-U-ABCD1234")]
            public void GenerateKey_WithValidIssueProperties_ReturnExpectedKey
                   (string desc, Priority priority, Category category, string createdAt, string expected)
            {
                //arrange
                var sut = new Issue(desc, priority, category, DateTime.Parse(createdAt));

                //act
                //use reflection to acces GenerateKey method

                MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

                var actual = methodInfo.Invoke(sut, null).ToString();

                //assert
                Assert.Equal(expected.Substring(0, 10), actual.Substring(0, 10));
            }



            #endregion
        }
    
}
