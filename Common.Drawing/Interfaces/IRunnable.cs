namespace Ujeby.Common.Drawing.Interfaces
{
	public interface IRunnable
	{
		void Run(Func<bool> handleInput);
	}
}