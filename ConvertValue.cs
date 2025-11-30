using UnityEngine;
public class ConvertStickInputTo3D
{
    /// <summary>
    /// 移動操作を3D空間に正規化
    /// </summary>
    /// <param name="_stickValue"></param>
    /// <returns></returns>
    public static Vector3 GetMoveVelocity(Vector2 _stickValue)
    {

        Vector3 _ret = Vector3.zero;
        _ret.x = _stickValue.x;
        _ret.z = _stickValue.y;

        return _ret;
    }

    /// <summary>
    /// カメラ操作をゲーム画面に正規化
    /// </summary>
    /// <param name="_stickValue"></param>
    /// <returns></returns>
    public static Vector3 GetLookVelocity(Vector2 _stickValue)
    {
        Vector3 _ret = Vector3.zero;
        _ret.x = _stickValue.y * -1;
        _ret.y = _stickValue.x;

        return _ret;
    }


    /// <summary>
    /// 引数の方向に移動方向を正規化した値
    /// <br/>カメラ等を入れる
    /// </summary>
    /// <param name="_transformRef"></param>
    /// <returns></returns>
    public static Vector3 GetNormalizedMoveVelocity(Vector3 _right, Vector3 _forward, Vector3 _moveVelocity)
    {

        Vector3 nor = Vector3.zero;
        // 高さを除いたベクトルに変換
        nor = (_right * _moveVelocity.x) + (_forward * _moveVelocity.z);
        nor.y = 0;
        nor.Normalize();
        return nor;
    }

}
