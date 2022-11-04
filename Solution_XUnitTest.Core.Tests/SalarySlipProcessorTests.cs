using Moq;
using Solution_XUnitTest.Core.Test1;
using System;
using Xunit;

namespace Solution_XUnitTest.Core.Tests
{
    public class SalarySlipProcessorTests
    {
        #region 1 CalculateBasicSalary

        [Fact]
        public void CalculateBasicSalary_EmployeeIsNull_ReturnArgumentNullException()
        {
            //tripple A
            //Arrange (preparation)
            Employee employee = null;

            //Act (lancement)
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateBasicSalary(e);


            //Assert (verification)
            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculateBasicSalary_ForEmployeeWageAndWorkingDays_ReturnBasicSalary()
        {
            //tripple A
            //Arrange (preparation)
            var employee = new Employee() { Wage = 500m, WorkingDays = 20 };

            //act (lancement)
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateBasicSalary(employee);
            var expected = 10000m;

            //assert (verification)
            Assert.Equal(expected, actual);
        }

        #endregion

        #region 2 CalculateTransportationAllowece

        [Fact]
        public void CalculateTransportationAllowece_EmployeeIsNull_ReturnArgumentNullException()
        {
            //arrange
            Employee employee = null;


            //act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateTransportationAllowece(e);

            //assert
            Assert.Throws<ArgumentNullException>(() => func(employee));
        }


        [Fact]
        public void CalculateTransportationAllowece_EmployeeWithOfficePlatform_ReturnTransportationAllowanceAmount()
        {
            //arrange
            var employee = new Employee { WorkPlatform = WorkPlatform.Office };


            //act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = Constants.TransportationAllowanceAmount;
            //assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void CalculateTransportationAllowece_EmployeeWithRemotePlatform_ReturnZero()
        {
            //arrange
            var employee = new Employee { WorkPlatform = WorkPlatform.Remote };


            //act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = 0m;
            //assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void CalculateTransportationAllowece_EmployeeHybridPlatform_ReturnTransportationAllowanceAmount()
        {
            //arrange
            var employee = new Employee { WorkPlatform = WorkPlatform.Hybrid };
            var salarySlipProcessor = new SalarySlipProcessor(null);

            //act
            var actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var expected = Constants.TransportationAllowanceAmount / 2;
            //assert
            Assert.Equal(actual, expected);
        }

        #endregion

        #region 3 CalculateDangerPay

        [Fact]
        public void CalculateDangerPay_EmployeeIsNull_ReturnArgumentNullException()
        {
            //arrange
            Employee employee = null;

            //act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (e) => salarySlipProcessor.CalculateDangerPay(e);

            //assert
            Assert.Throws<ArgumentNullException>(() => func(employee));

        }


        [Fact]
        public void CalculateDangerPay_EmployeeIsDanger_ReturnDangerPay()
        {
            //arrange
            var employee = new Employee { IsDanger = true };

            //act
            var salarySlipProcessor = new SalarySlipProcessor(null);
            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = Constants.DangerPayAmount;
            //assert

            Assert.Equal(actual, expected);
        }


        [Fact]
        public void CalculateDangerPay_EmployeeDangerOffAndInDangerZone_ReturnDangerPay()
        {
            //arrange
            var employee = new Employee { IsDanger = false, DutyStation = "Ukraine" };
            //je mock le service zone service
            var mock = new Mock<IZoneService>();
            //j'imagine que le service me retourne true suite a son virtuel execution
            var setup = mock.Setup(z => z.IsDangerZone(employee.DutyStation)).Returns(true);

            //act
            var salarySlipProcessor = new SalarySlipProcessor(mock.Object);
            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = Constants.DangerPayAmount;

            //assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void CalculateDangerPay_EmployeeDangerOffAndNotInDangerZone_ReturnZero()
        {
            //arrange
            var employee = new Employee { IsDanger = false, DutyStation = "Canada" };
            //je mock le service zone service
            var mock = new Mock<IZoneService>();
            //j'imagine que le service me retourne true suite a son virtuel execution
            var setup = mock.Setup(z => z.IsDangerZone(employee.DutyStation)).Returns(false);

            //act
            var salarySlipProcessor = new SalarySlipProcessor(mock.Object);
            var actual = salarySlipProcessor.CalculateDangerPay(employee);
            var expected = 0m;

            //assert
            Assert.Equal(actual, expected);

        }

        #endregion
    }
}
