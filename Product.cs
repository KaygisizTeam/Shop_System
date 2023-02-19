using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Product : MonoBehaviour
{
    public delegate void OnProductBoughtDelegate();
    public static event OnProductBoughtDelegate OnProductBoughtEvent;
    private enum SkinStatus
    {
        Bought, OnSale, Equipped, Equip,
    }
    private SkinStatus skinStatus;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _skin;
    [SerializeField] private Image _skinImg;
    [SerializeField] private Button _buyButton;
    [SerializeField] private int skinID;
    private void OnEnable()
    {
        // ürün satın alınınca tetiklenecek evente, satın alma butonunun özelliklerini güncelleyen metodu abone ediyoruz.
        OnProductBoughtEvent += UpdateButtons;
    }
    private void OnDisable()
    {
        OnProductBoughtEvent -= UpdateButtons;
    }
    private void Awake()
    {
        // ürünün varsayılan durumunu alınmamış olarak ayarlıyoruz.
        ChangeSkinStatus(SkinStatus.OnSale);

        // eğer bu ürünün id'sine uyan bir kayıt varsa durumu satın alınmış olarak değiştiriyoruz.
        if (HasSkin()) ChangeSkinStatus(SkinStatus.Bought);
    }
    private void Start()
    {
        // satın alma butonunun özelliklerini ürün durumuna göre belirliyoruz.
        UpdateButtons();
        PlayerPrefs.GetInt("skinID", 1);
    }
    private void UpdateButtons()
    {
        switch (skinStatus)
        {
            
            case SkinStatus.Bought:
                _buyButton.GetComponentInChildren<Text>().text = "equip";
                break;
            //skin secildiyse equipped yapmasi icin yazdim ama deneme suan calimiyor
            case SkinStatus.Equip:
                _buyButton.GetComponentInChildren<Text>().text = "Equipped";
                break;
            
            case SkinStatus.OnSale:
                _buyButton.GetComponentInChildren<Text>().text = "buy";
                break;
        }
    }
    public void OnBuyButtonPress()
    {
        switch (skinStatus)
        {
            // eğer ürün alınmışsa karakterimizin objesine bağlı olan SkinLoader kodu içindeki...
            // static colorToLoad değişkenini bu ürünün materyaline atıyoruz.
            // karakterimiz başka bir sahnede olduğu için, karakter materyal değişkeni static.
            case SkinStatus.Bought:
                SkinLoader.skinToLoad = _skin;
                break;

            case SkinStatus.OnSale:
                // mevcut bakiye yeterli değilse hiçbir işlem yapmıyoruz.
                if (!PlayerMoney.TryBuySkin(_price)) return;

                // mevcut bakiyeden ürünün fiyatını eksiltiyoruz.
                PlayerMoney.DecreaseCurrency(_price);

                // ürünü kayıtlara ekliyoruz.
                SaveSkin();

                // ürün durumunu satın alındı olarak değiştiriyoruz.
                ChangeSkinStatus(SkinStatus.Bought);
                ChangeSkinStatus(SkinStatus.Equip);
                
                UpdateButtons();

                // bu evente abone olan buton özelliklerini güncelleme metodu (UpdateButtons) ve
                // UIManager kodundaki bakiye textini güncelleme metotu (UpdateCurrencyText) çalışıyor.
                OnProductBoughtEvent?.Invoke();
                break;
        }
    }
    
    private void ChangeSkinStatus(SkinStatus skinStatus)
    {
        // satın alınma durumunun değiştirilmesi.
        this.skinStatus = skinStatus;
    }
    private void SaveSkin()
    {
        // her ürünün kendine has ürün numarası (ID) var.
        PlayerPrefs.SetInt(skinID.ToString(), 1);
    }
    private bool HasSkin()
    {
        // kayıtlarda bu ürün numarasına sahip ürün var mı?
        return PlayerPrefs.HasKey(skinID.ToString());
    }
}
