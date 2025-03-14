export async function defaultPanel() {
    console.log("Click")
    const response = await fetch("/api/panels", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            "name": "default panel",
            "size": "150",
            "SampleRate": "0.005",
            "Distributions": {
                "sex": {
                    "m": 0.4,
                    "v": 0.6
                },
                "leef": {
                    "20": 0.2,
                    "30": 0.6,
                    "40": 0.2
                }
            },
            "CitizenCount":10000,
            "ReservePercentage":0.2,
            "ResponseRate":0.005
        }),
    });
}
