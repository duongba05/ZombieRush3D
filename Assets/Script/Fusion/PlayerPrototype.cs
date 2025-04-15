using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrototype : NetworkBehaviour
{
    private const int MAX_HEALTH = 100; // Giới hạn máu tối đa
    private const int MAX_MANA = 100;   // Giới hạn mana tối đa
    private const int DAMAGE = 10;      // Sát thương mất đi mỗi lần nhấn Space
    private const int MANA_COST = 20;   // Lượng mana giảm mỗi lần nhấn Space

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public PlayerInfo PlayerInfo { get; set; }

    public Slider healthSlider;
    public Slider manaSlider;

    private void Start()
    {
        if (HasStateAuthority)
        {
            // Khởi tạo máu và mana ban đầu
            PlayerInfo = new PlayerInfo
            {
                health = MAX_HEALTH,
                mana = MAX_MANA,
                score = 0
            };
        }

        // Thiết lập maxValue cho thanh trượt
        healthSlider.maxValue = MAX_HEALTH;
        manaSlider.maxValue = MAX_MANA;

        // Cập nhật giá trị ban đầu của thanh máu/mana
        healthSlider.value = MAX_HEALTH;
        manaSlider.value = MAX_MANA;
    }

    private void OnHealthChanged()
    {
        healthSlider.value = PlayerInfo.health;
        manaSlider.value = PlayerInfo.mana;
    }

    private void Update()
    {
        if (HasStateAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Giảm máu và mana, nhưng không thấp hơn 0
                var newHealth = Mathf.Max(0, PlayerInfo.health - DAMAGE);
                var newMana = Mathf.Max(0, PlayerInfo.mana - MANA_COST);

                PlayerInfo = new PlayerInfo
                {
                    health = newHealth,
                    mana = newMana,
                    score = 100
                };
            }
        }
    }
}
