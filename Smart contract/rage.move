module rage_effect::rage {
    use std::option;
    use std::string::{Self, String};

    use nft_protocol::collection::{Self, Collection};
    use nft_protocol::creators;
    use nft_protocol::display_info;
    use nft_protocol::mint_cap::{Self, MintCap};
    use nft_protocol::mint_event;
    use nft_protocol::tags;
    use ob_permissions::witness::Self as ob_permissions_witness;
    use ob_request::request::{Policy, PolicyCap, WithNft};
    use ob_request::transfer_request::Self as ob_transfer_request;
    use ob_request::withdraw_request::{Self as ob_withdraw_request, WITHDRAW_REQ};
    use ob_utils::display as ob_display;
    use ob_utils::utils;
    use sui::display;
    use sui::object::{Self, UID};
    use sui::package::Publisher;
    use sui::transfer;
    use sui::tx_context::{Self, TxContext};
    use sui::url::{Self, Url};

    struct Witness has drop {}

    struct RAGE has drop {}

    struct AdminCap has key, store {
        id: UID
    }

    struct Rage has key, store {
        id: UID,
        name: String,
        description: String,
        image_url: Url,
        metadata_url: Url
    }


    fun init(otw: RAGE, ctx: &mut TxContext) {
        let sender: address = tx_context::sender(ctx);

        // collection
        let (collection, mint_cap): (Collection<Rage>, MintCap<Rage>) = collection::create_with_mint_cap<RAGE, Rage>(
            &otw, option::none(), ctx
        );

        let publisher: Publisher = sui::package::claim(otw, ctx);
        let dw = ob_permissions_witness::from_witness<Rage, Witness>(Witness {});

        let adminCap: AdminCap = AdminCap {
            id: object::new(ctx)
        };

        // display
        let tags: vector<String> = vector[tags::art(), tags::collectible(), tags::game_asset()];

        let display = display::new<Rage>(&publisher, ctx);
        display::add(&mut display, string::utf8(b"name"), string::utf8(b"{name}"));
        display::add(&mut display, string::utf8(b"description"), string::utf8(b"{description}"));
        display::add(&mut display, string::utf8(b"image_url"), string::utf8(b"{image_url}"));
        display::add(&mut display, string::utf8(b"metadata_url"), string::utf8(b"{metadata_url}"));
        display::add(&mut display, string::utf8(b"tags"), ob_display::from_vec(tags));
        display::add(&mut display, string::utf8(b"collection_id"), ob_display::id_to_string(&object::id(&collection)));
        display::update_version(&mut display);
        transfer::public_transfer(display, sender);

        // Add name and description to Collection
        collection::add_domain(
            dw,
            &mut collection,
            display_info::new(
                string::utf8(b"Rage Effect"),
                string::utf8(b"Rage Effect"),
            ),
        );

        let creators = vector[ sender ];

        collection::add_domain(
            dw,
            &mut collection,
            creators::new(utils::vec_set_from_vec(&creators)),
        );
        let (transfer_policy, transfer_policy_cap) = ob_transfer_request::init_policy<Rage>(&publisher, ctx);


        let (policy, policy_cap): (Policy<WithNft<Rage, WITHDRAW_REQ>>, PolicyCap) = ob_withdraw_request::init_policy<Rage>(
            &publisher,
            ctx
        );

        transfer::public_transfer(publisher, sender);
        transfer::public_transfer(adminCap, sender);

        transfer::public_share_object(collection);
        transfer::public_transfer(mint_cap, sender);

        transfer::public_share_object(transfer_policy);
        transfer::public_transfer(transfer_policy_cap, sender);

        transfer::public_share_object(policy);
        transfer::public_transfer(policy_cap, sender);
    }

    public entry fun mint(
        mintCap: &mut MintCap<Rage>,
        name: vector<u8>,
        description: vector<u8>,
        imageUrl: vector<u8>,
        metadataUrl: vector<u8>,
        receiver: address,
        ctx: &mut TxContext
    ) {
        let rage: Rage = Rage {
            id: object::new(ctx),
            name: string::utf8(name),
            description: string::utf8(description),
            image_url: url::new_unsafe_from_bytes(imageUrl),
            metadata_url: url::new_unsafe_from_bytes(metadataUrl)
        };

        mint_event::emit_mint(
            ob_permissions_witness::from_witness(Witness {}),
            mint_cap::collection_id(mintCap),
            &rage,
        );

        transfer::public_transfer(rage, receiver);
    }
}
