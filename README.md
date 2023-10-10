# RageOnSui
Rage Effect on Sui

# Test Gamers’ Wallet Accounts

All gamer accounts have 1 Sui in them.

Public Keys

0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99
0x935bbd29b8fee386639b9342df70616f3a591d477ad72c538de402cb116f3f8f
0x7ebc321a69830dfac162819fd66645d7640120d30e7446928f80d4d7c5f1358d
0xeb86df9ccaff085797c73180e82e2d11e1bff2b562bf0993bbc1fd395fa07187
0x6a3c4f4937563a20204325e9e5767a40b3af6f239aa2ea57b7987573ca11f4bf

Private Keys

0xff3ba01881208b1604e6f1a07441e5fc985f68abaffccf47903d43036c390d60
0x83b051499e771cb802fed598c92bb86ea5a7a17e0807756b6b02197f59653982
0xefb2706e4b749aa9b0194084f9d2859cb65bfa1b715ad98c2739f428241c3d88
0xf4c13dcc173c0edfa2832fe124e1a45d671f1ed175d3f9f881edf98cf95d825c
0xe0b19eb2366513a97385555e54669d890e1e65cc5da2cb09eb496cb3a91f920c

## Mint SUI token

POST: https://api-staging.mirrorworld.fun/v2/sui/testnet/asset/rage-effect/mint

Request Body:

- name
    - NFT name
- description
    - NFT description. Stored on-chain
- image_url
- metadata_url
- to_wallet_address
    - can be any wallet address to receive the minted NFT
- fee_payer_wallet
    - **public_key from above test wallets**

Sample cURL:

```
curl --location 'https://api-staging.mirrorworld.fun/v2/sui/testnet/asset/rage-effect/mint'
--header 'x-api-key: your-api-key'
--header 'Content-Type: application/json'
--data '{
    "name": "Rage Effect",
    "description": "Rage Effect Test Mint",
    "image_url": "https://api.rageeffect.io/php/nft/image/geo_us.jpg",
    "metadata_url": "https://api.rageeffect.io/php/nft/script/metadata/wallet_01/geo-tagged/1/geo_nft_metadata.json",
    "to_wallet_address": "0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99",
    "fee_payer_wallet": "0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99"
}'
```

## Get SUI Token

GET: https://api-staging.mirrorworld.fun/v2/sui/testnet/asset/rage-effect/find/:nft_object_id

## Get All SUI Token Owned

GET: https://api.mirrorworld.fun/v2/sui/testnet/asset/nft/owner

Request Body:
-   owner_address
    - can be any wallet address to fetch NFTs

Sample cURL:

```
curl --location 'https://api.mirrorworld.fun/v2/sui/testnet/asset/nft/owner'
--header 'x-api-key: your-api-key'
--header 'Content-Type: application/json'
--data '{
    "owner_address": "0xab34e27430c9216a30080ed95c42e2432a87706fb186330abb5a3c244e16fa99"
}'
```

## Get Current Location Information:

GET: http://www.geoplugin.net/php.gp?ip={ipAddress}


## NFT Categories

- #### Geo-Tagged NFT
Geo-tagged NFTs are digital assets with attributes that dynamically adjust in response to the geographical position of a user. These tokens are designed to synchronize their properties with the real-world location of the interacting player.

More Info: https://docs.google.com/document/d/1qnKG8rZTU_Yrx9Nf4g3Hwu4w1N8cgfneb6kuc2Ebjzk/edit?usp=sharing

- #### Dynamic NFT
Dynamic NFTs change as you play the game, based on what you achieve or what happens in the game. This adds excitement and keeps things unpredictable

More Info: https://docs.google.com/document/d/1V86I_IjmsV9Kp-_8BXXkt0IeALomfbeeThSPcJlBgts/edit?usp=sharing 

- #### Hide n Seek/Questing NFT
Questing NFT is a type of NFT that can be staked or put into the game with a chance of being randomly discovered by other users, up to a maximum of 10% in any match. When users come across these NFTs while playing the game, they and the staker receive X tokens as a bonus reward. Essentially, it's like a treasure hunt within the game where players can find and earn extra tokens by discovering these special NFTs.

More Info: https://docs.google.com/document/d/10DyFVPBlEa4v2z1M1blT6aCNMzE6jBPDsbS4Yj6VnaY/edit?usp=sharing

