using System.Threading.Tasks;


/// <summary>
/// ���� ��� ���� �ܼ� �񵿱� ����� �������̽�, �� �������̽��� �־�� �ʱ�ȭ ����
/// </summary>
public interface IAsyncElement
{
    Task Init();
}

/// <summary>
/// ������ �ʿ��� �񵿱� ����� �������̽�(Priority�� �������� ���� ����)
/// </summary>
public interface IAsyncElementWithPriority : IAsyncElement
{
    int Priority { get; } //�������� ���� ����
}

//DropDown�� ������ �ʿ��� Ŭ������ �ִٸ� ������ IAsyncElementWithPriority�� �����ϴ� ��� (ISP)
public interface IDropDown : IAsyncElement
{
    public void OnValueChanged(int value);
}

//Slider�� �񵿱� ��û�� ���� Txt2img ��ü�� �����ϴ� ���̹Ƿ� IAsyncElement�� �ʿ� ����
public interface ISlider
{
    public void OnValueChanged(float value);
    public void OnInputfield(string value);
}