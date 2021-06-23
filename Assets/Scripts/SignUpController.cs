using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth; // 계정인증기능 사용

public class SignUpController : MonoBehaviour
{
    public InputField emailField;
    public InputField password1Field;
    public InputField password2Field;
    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;  // 인증 객체 초기화
    }
 
    public void SignUpButtonClicked()  // 해당 이메일,비밀번호로 가입하기
    {
        SignUp(emailField.text, password1Field.text);
    }

    private void SignUp(string email, string password)
    {
        // 이메일과 비밀번호로 가입하는 함수
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
             task => {
                 if (!task.IsCanceled && !task.IsFaulted)
                 {
                     Debug.Log(email + " 로 회원가입 하셨습니다.");
                 }
                 else
                 {
                     Debug.Log("회원가입에 실패하셨습니다.");
                 }
             }
        );
    }

    public void BackButtonClicked()
    {
        SceneManager.LoadScene("LogIn");
    }
}
