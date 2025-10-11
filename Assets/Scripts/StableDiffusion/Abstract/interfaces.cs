using System.Threading.Tasks;


/// <summary>
/// 순서 상관 없고 단순 비동기 요소의 인터페이스, 이 인터페이스가 있어야 초기화 가능
/// </summary>
public interface IAsyncElement
{
    Task Init();
}

/// <summary>
/// 순서가 필요한 비동기 요소의 인터페이스(Priority가 낮을수록 먼저 실행)
/// </summary>
public interface IAsyncElementWithPriority : IAsyncElement
{
    int Priority { get; } //낮을수록 먼저 실행
}

//DropDown중 순서가 필요한 클래스가 있다면 별도로 IAsyncElementWithPriority를 구현하는 방식 (ISP)
public interface IDropDown : IAsyncElement
{
    public void OnValueChanged(int value);
}

//Slider는 비동기 요청이 없고 Txt2img 객체를 수정하는 것이므로 IAsyncElement가 필요 없음
public interface ISlider
{
    public void OnValueChanged(float value);
    public void OnInputfield(string value);
}